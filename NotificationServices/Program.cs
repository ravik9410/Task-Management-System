using NotificationServices.Repository;
using NotificationServices.Services;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IRabbitMQRepository,RabbitMQRepository>();
var app = builder.Build();
var factory = new ConnectionFactory();
var connnection = factory.CreateConnection();
var channel = connnection.CreateModel();
channel.QueueDeclare("demo-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, e) =>
{
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    IRabbitMQRepository rabbitMQRepository = new RabbitMQRepository();
    rabbitMQRepository.SendMail(JsonConvert.DeserializeObject<object>(message));
    Console.WriteLine(JsonConvert.DeserializeObject<object>(message));
};
channel.BasicConsume("demo-queue", true, consumer);
Console.ReadLine();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

