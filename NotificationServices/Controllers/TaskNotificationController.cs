using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NotificationServices.Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class TaskNotificationController : ControllerBase
    {
        [HttpGet]
        public IActionResult SendNotification()
        {
            return Ok();
        }
    }
}
