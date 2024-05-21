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
        var modifyservices = scope.ServiceProvider.GetRequiredService<TaskModifyService>();
        await modifyservices.ModifyTaskStatus(JsonConvert.DeserializeObject<AssignedUserTask>(message)!);
        email = modifyservices.GetUserDetailsById(email).GetAwaiter().GetResult();
    }
    IRabbitMQRepository rabbitMQRepository = new RabbitMQRepository(builder.Configuration);
    rabbitMQRepository.SendMail(JsonConvert.DeserializeObject<object>(message), email);
    Console.WriteLine(JsonConvert.DeserializeObject<object>(message));


};
channel.BasicConsume("demo-queue", true, consumer);
//Console.ReadLine();
// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

