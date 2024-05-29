using NotificationServices.Repository;
using NotificationServices.Services;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using NotificationServices.Utility;
using AssigneTaskServices.Models;
using NotificationServices.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IRabbitMQRepository, RabbitMQRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<BackendApiAuthenticationHttpClientHandler>();
builder.Services.AddScoped<ITaskUpdation, TaskModifyService>();
builder.Services.AddHttpClient("ModifyTaskStatus", config => { config.BaseAddress = new Uri(builder.Configuration["ServiceUrls:TaskAPI"]); }).AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();
builder.Services.AddHttpClient("GetUserById", config => { config.BaseAddress = new Uri(builder.Configuration["ServiceUrls:UserAPI"]); }).AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication();
builder.AddJwtAuthenticationServices();
var app = builder.Build();
var factory = new ConnectionFactory();
var connnection = factory.CreateConnection();
var channel = connnection.CreateModel();
channel.QueueDeclare("demo-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += async (sender, e) =>
{
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    string email = string.Empty;
    using (var scope = app.Services.CreateScope())
    {
        // var modifyservices = scope.ServiceProvider.GetRequiredService<TaskModifyService>();
        ITaskUpdation modifyservices = new TaskModifyService(scope.ServiceProvider);
        await modifyservices.ModifyTaskStatus(JsonConvert.DeserializeObject<AssignedUserTask>(message)!);
        email = "ravik5@chetu.com";//modifyservices.GetUserDetailsById(email).GetAwaiter().GetResult();
    }
    IRabbitMQRepository rabbitMQRepository = new RabbitMQRepository(builder.Configuration);
    rabbitMQRepository.SendMail(JsonConvert.DeserializeObject<object>(message), email);
    Console.WriteLine(JsonConvert.DeserializeObject<object>(message));


};
channel.BasicConsume("demo-queue", true, consumer);
//Console.ReadLine();
// Configure the HTTP request pipeline.


app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Notification Services");
    c.InjectStylesheet("/swagger/custom.css");
    c.RoutePrefix = String.Empty;
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

