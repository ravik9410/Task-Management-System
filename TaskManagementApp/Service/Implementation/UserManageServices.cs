using Newtonsoft.Json;
using TaskManagementApp.Models.DTO;
using TaskManagementApp.Service.Contract;
using static TaskManagementApp.Utility.StaticData;

namespace TaskManagementApp.Service.Implementation
{
	public class UserManageServices : IManageUser
	{
		private readonly IBaseServices _baseService;
		private readonly ITokenHandler _tokenHandler;
		private readonly IHttpContextAccessor _contextAccessor;
		public UserManageServices(IBaseServices baseService, ITokenHandler tokenHandler, IHttpContextAccessor contextAccessor)
		{
			_baseService = baseService;
			_tokenHandler = tokenHandler;
			_contextAccessor = contextAccessor;
		}

		public async Task<string> DeleteUser(string userId)
		{
			var result = await _baseService.SendAsync(new()
			{
				ApiType = ApiType.DELETE,
				Url = AuthUrl + "/api/usermanagement/" + userId,
			});
			return result?.Message!;

		}

		public async Task<List<UserDto>> GetAllUser()
		{
			var result = await _baseService.SendAsync(new()
			{
				ApiType = ApiType.GET,
				Url = AuthUrl + "/api/usermanagement",
			});
			if (result!.IsSuccess)
			{
				var responseContent = JsonConvert.DeserializeObject<List<UserDto>>(result.Result?.ToString()!);

				return responseContent!;
				//login success
			}
			else
			{
				return new();
			}
		}

		public async Task<UserDto> GetUserById(string userId)
		{
			var result = await _baseService.SendAsync(new()
			{
				ApiType = ApiType.GET,
				Url = AuthUrl + "/api/usermanagement/" + userId,
			});
			if (result!.IsSuccess)
			{
				var responseContent = JsonConvert.DeserializeObject<UserDto>(result.Result?.ToString()!);

				return responseContent!;
				//login success
			}
			else
			{
				return new();
			}
		}

		public async Task<string> UpdateUser(UserDto user, string userId)
		{
			var result = await _baseService.SendAsync(new()
			{
				ApiType = ApiType.PUT,
				Url = AuthUrl + "/api/usermanagement/" + userId,
				Data = user
			});

			return result?.Message!;

		}
	}
}
