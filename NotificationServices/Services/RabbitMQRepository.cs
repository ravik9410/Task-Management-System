using Newtonsoft.Json;
using NotificationServices.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace NotificationServices.Services
{
    public class RabbitMQRepository : IRabbitMQRepository
    {
        public async Task<int> ReceiveAsync()
        {

            var factory = new ConnectionFactory();
            var connnection = factory.CreateConnection();
            var channel = connnection.CreateModel();
            channel.QueueDeclare("demo-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(JsonConvert.DeserializeObject<object>(message));
            };
            channel.BasicConsume("demo-queue", true, consumer);
            Console.ReadLine();
            int result = 1;
            return result;
        }

        public async Task SendMail(object value)
        {
            var fromAddress = new MailAddress("from@gmail.com", "From Name");
            var toAddress = new MailAddress("to@example.com", "To Name");
            const string fromPassword = "fromPassword";
            const string subject = "Subject";
            const string body = "Body";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);

            }


        }
    }
}
