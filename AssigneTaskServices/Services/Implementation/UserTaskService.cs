using AssigneTaskServices.Models.DTO;
using AssigneTaskServices.Services.Contract;
using Newtonsoft.Json;

namespace AssigneTaskServices.Services.Implementation
{
    public class UserTaskService : IUserTaskServices
    {
        private readonly IHttpClientFactory _httpClient;

        public UserTaskService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserTaskDto> GetTaskById(int taskId)
        {
            try
            {
                var client = _httpClient.CreateClient("GetTaskById");
                var data = await client.GetAsync("/task?taskId=" + taskId);
                var content = await data.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<ResponseDto>(content) ?? new();
                if (response.IsSuccess)
                {
                    var taskDto = JsonConvert.DeserializeObject<UserTaskDto>(response.Result?.ToString()!);
                    return taskDto!;
                }
                return new();
            }
            catch (Exception ex)
            {
                throw;
                //return new();
            }
        }
    }
}
