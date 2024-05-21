using AssigneTaskServices.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NotificationServices.Models.DTO;
using System.Text;

namespace NotificationServices.Services
{
    public class TaskModifyService : ITaskUpdation
    {
        private readonly IServiceProvider _service;

        public TaskModifyService(IServiceProvider service)
        {
            _service = service;
        }

        public async Task<bool> ModifyTaskStatus(AssignedUserTask task)
        {
            var _httpClientFactory = _service.GetRequiredService<IHttpClientFactory>();
            var httpcontext = _service.GetRequiredService<IHttpContextAccessor>();
            var client = _httpClientFactory.CreateClient("ModifyTaskStatus");
            //var client = _httpClientFactory.CreateClient();

            //    var requestMessage = new HttpRequestMessage();
            // requestMessage.Method = HttpMethod.Patch;
            //have to implement the delete functionality for task.
            //  requestMessage.RequestUri = new Uri("/api/task");

            //  requestMessage.Content = new StringContent(JsonConvert.SerializeObject(task).ToString());
            var content = new StringContent(JsonConvert.SerializeObject(task).ToString(),Encoding.UTF8,"application/json");
            // var token = await httpcontext.HttpContext.GetTokenAsync("access_token");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var data = await client.PatchAsync("/api/task", content);
            var response = await data.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<JObject>(response);
            //  var response = await client.SendAsync(requestMessage);
            // var result = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
            if (result["Result"]?.ToString() == "Task saved successfully.")
            {
                return true;
            }
            return false;
        }

        public async Task<string> GetUserDetailsById(string userId)
        {
            var _httpClientFactory = _service.GetRequiredService<IHttpClientFactory>();
            var client = _httpClientFactory.CreateClient("GetUserById");
            var requestMessage = new HttpRequestMessage();
            requestMessage.RequestUri = new Uri("/api/auth/" + userId);
            requestMessage.Method = HttpMethod.Get;
            //requestMessage.Content = new StringContent(JsonConvert.SerializeObject(task).ToString());
            var response = await client.SendAsync(requestMessage);
            var result = JsonConvert.DeserializeObject<ResponseDto>(await response.Content.ReadAsStringAsync());
            if (result.IsSuccess)
            {
                var res = JsonConvert.DeserializeObject<JObject>(result.Result.ToString());
                return res["Email"].ToString();
                //   return true;
            }
            return string.Empty;
            // return false;
        }
    }
}
