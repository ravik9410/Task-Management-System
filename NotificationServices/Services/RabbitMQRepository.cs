using Newtonsoft.Json;
using NotificationServices.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Mail;
using System.Net;
using System.Text;
using AutoMapper;

namespace NotificationServices.Services
{
    public class RabbitMQRepository : IRabbitMQRepository
    {
        private readonly IConfiguration _IConfigure;
        public RabbitMQRepository(IConfiguration configuration)
        {
            _IConfigure = configuration;
        }
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

        public async Task SendMail(object value, string emailAddress)
        {
            /* var fromAddress = new MailAddress("from@gmail.com", "From Name");
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

             }*/

            string SendingByEmail = _IConfigure.GetSection("EmailCredential").GetSection("SendingByEmail").Value;
            string Password = _IConfigure.GetSection("EmailCredential").GetSection("Password").Value;
            MailMessage mm = new();
            /*for (int listCounter = 0; listCounter < shareEntities.Count; listCounter++)
            {
                MailAddress? toMail = new(shareEntities[listCounter].userEmail!.ToString());
                mm.To.Add(toMail);
            }*/
            mm.To.Add(emailAddress);
            mm.From = new MailAddress(SendingByEmail);
            mm.Subject = "Notification for Task";
            mm.IsBodyHtml = true;
            mm.CC.Add("ravik5@chetu.com");
            /* GetFileDetailsByIdModel data = new();
             data = GetFileDetailsById(shareEntities[0].FileId);*/
            string body = "Hi ,Task Assigned to you by ravi.";// BuildEmailBodyForSharingFiles(data, shareEntities[0].Editable, userName);
            if (body != string.Empty)
            {
                mm.Body = body;
            }

            SmtpClient smtp = new()
            {
                Port = 587,
                Host = "smtp.gmail.com",
                Timeout = 10000,
                EnableSsl = true
            };
            NetworkCredential credential = new(SendingByEmail, Password);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = credential;
            smtp.Send(mm);
        }
    }
}
