using Newtonsoft.Json;
using TaskCreationService.Models.DTO;
using TaskCreationService.Services.Contract;

namespace TaskCreationService.Services.Implementation
{
    public class UserDetailsServices : IGetUserDetailsServices
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserDetailsServices(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }

        public async Task<UserDto> GetById(string userId)
        {

            var client = _httpClientFactory.CreateClient("GetUser");
            var response = await client.GetAsync($"/api/usermanagement/"+userId);
            var apiContet = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContet);
            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<UserDto>(Convert.ToString(resp.Result)!)?? new();
            }
            return new UserDto();
        }
      
    }

}
