using AssigneTaskServices.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NotificationServices.Models.DTO;
using System.Text.Json.Nodes;

namespace NotificationServices.Services
{
    public class TaskModifyService : ITaskUpdation
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TaskModifyService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> ModifyTaskStatus(AssignedUserTask task)
        {
            var client = _httpClientFactory.CreateClient("ModifyTaskStatus");
            var requestMessage = new HttpRequestMessage();
            requestMessage.Method = HttpMethod.Patch;
            requestMessage.RequestUri = new Uri("/api/");
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(task).ToString());
            var response = await client.SendAsync(requestMessage);
            var result = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
            if (result == "Task saved successfully.")
            {
                return true;
            }
            return false;
        }
        public async Task<string> GetUserDetailsById(string userId)
        {
            var client = _httpClientFactory.CreateClient("GetUserById");
            var requestMessage = new HttpRequestMessage();
            requestMessage.RequestUri = new Uri("/api/usermanagement?userId=" + userId);
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
