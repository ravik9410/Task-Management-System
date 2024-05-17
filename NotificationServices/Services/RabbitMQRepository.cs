using Newtonsoft.Json;
using NotificationServices.Repository;
using RabbitMQ.Client;
using System.Text;

namespace NotificationServices.Services
{
    public class RabbitMQRepository<T> : IRabbitMQRepository<T>
    {
        public async Task<int> SendAsync(T request)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                //Uri = new Uri("amqp://guest:guest@localhost:15672/")
            };
            using var connnection = factory.CreateConnection();
            var channel = connnection.CreateModel();
            channel.QueueDeclare("demo-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            var message = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
            channel.BasicPublish("", "demo-queue", null, message);
            int result = 1;
            return result;
           // throw new NotImplementedException();
        }
    }
}
