using Newtonsoft.Json;
using System.Collections.Generic;
using TaskDashboardServices.Models;
using TaskDashboardServices.Models.DTO;
using TaskDashboardServices.Services.Contract;

namespace TaskDashboardServices.Services.Implementation
{
    public class TaskDashboard : ITaskDashboard
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TaskDashboard(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<UserTaskDto>> GetAllTask()
        {
            var client = _httpClientFactory.CreateClient("GetTask");
            var response = await client.GetAsync("/api/task");
            if (response.IsSuccessStatusCode)
            {
                var stringcontent = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseDto>((await response.Content.ReadAsStringAsync()).ToString());
                var result = JsonConvert.DeserializeObject<List<UserTaskDto>>(data.Result.ToString()!);
                return result!;
            }
            //response.StatusCode
            return new();
        }

        public async Task<List<UserTaskDto>> GetTaskByUserId(string id)
        {
            var client = _httpClientFactory.CreateClient("GetTask");
            var response = await client.GetAsync("/api/GetTaskByUserId?userId=" +id);
            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<ResponseDto>((await response.Content.ReadAsStringAsync()).ToString());
                var result = JsonConvert.DeserializeObject<List<UserTaskDto>>(data.Result.ToString()!);
                return result!;
            } 
            return new();
        }

        public async Task<List<UserTaskDto>> GetTaskListByStatus(string status)
        {
            var client = _httpClientFactory.CreateClient("GetTask");
            var response = await client.GetAsync("/api/GetTaskByStatus?status="+status);
            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<ResponseDto>((await response.Content.ReadAsStringAsync()).ToString());
                var result = JsonConvert.DeserializeObject<List<UserTaskDto>>(data.Result?.ToString()!);
                return result!;
            }
            return new();
        }
    }
}
