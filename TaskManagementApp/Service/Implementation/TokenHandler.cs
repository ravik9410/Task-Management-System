using TaskManagementApp.Service.Contract;
using TaskManagementApp.Utility;

namespace TaskManagementApp.Service.Implementation
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenHandler(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void ClearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(StaticData.JwtToken);
        }

        public string GetToken()
        {
            string token = string.Empty;
            bool? isValid = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(StaticData.JwtToken, out token!);
            return isValid is true ? token : string.Empty!;
        }

        public void SetToken(string token)
        {
            _contextAccessor.HttpContext?.Response.Cookies.Append(StaticData.JwtToken, token);
        }
    }
}
