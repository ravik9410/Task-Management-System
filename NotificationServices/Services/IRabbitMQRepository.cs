using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServices.Repository
{
	public interface IRabbitMQRepository
	{
		Task<int> ReceiveAsync();
		Task SendMail(object value);

    }
}
