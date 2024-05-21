using Newtonsoft.Json;
using System.Net;
using System.Text;
using TaskManagementApp.DTO.Models;
using TaskManagementApp.Models.DTO;
using TaskManagementApp.Service.Contract;
using static TaskManagementApp.Utility.StaticData;

namespace TaskManagementApp.Service.Implementation
{
    public class BaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private ResponseDto _responseDto;
        private readonly ITokenHandler _tokenHandler;

        public BaseService(IHttpClientFactory httpClientFactory, ResponseDto responseDto, ITokenHandler tokenHandler)
        {
            _httpClientFactory = httpClientFactory;
            _responseDto = responseDto;
            _tokenHandler = tokenHandler;
        }
        public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                HttpClient httpClient = new HttpClient(clientHandler);


                //HttpClient httpClient = _httpClientFactory.CreateClient("Api Call");
                HttpRequestMessage httpRequestMessage = new();
                httpRequestMessage.Headers.Add("Accept", "application/json");
                string token = _tokenHandler.GetToken();
                httpRequestMessage.Headers.Add("Authorization", $"Bearer {token}");
                httpRequestMessage.Headers.Add("Platform", "IOS");

                httpRequestMessage.RequestUri = new Uri(requestDto.Url!);
                if (requestDto.Data != null)
                {
                    httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }
                HttpResponseMessage? httpResponseMessage = null;
                switch (requestDto.ApiType)
                {
                    case ApiType.POST:
                        {
                            httpRequestMessage.Method = HttpMethod.Post;
                            break;
                        }
                    case ApiType.DELETE:
                        {
                            httpRequestMessage.Method = HttpMethod.Delete;
                            break;
                        }
                    case ApiType.PUT:
                        {
                            httpRequestMessage.Method = HttpMethod.Put;
                            break;
                        }
                    default:
                        {
                            httpRequestMessage.Method = HttpMethod.Get;
                            break;
                        }
                }
                httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                switch (httpResponseMessage.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        {
                            _responseDto.Message = "Not Found";
                            _responseDto.IsSuccess = false;
                            break;
                        }
                    case HttpStatusCode.BadRequest:
                        {
                            _responseDto.Message = "Bad Request";
                            _responseDto.IsSuccess = false;
                            break;
                        }
                    case HttpStatusCode.InternalServerError:
                        {
                            _responseDto.Message = "Internal Server Error";
                            _responseDto.IsSuccess = false;
                            break;
                        }
                    case HttpStatusCode.Unauthorized:
                        {
                            _responseDto.Message = "Unauthorized";
                            _responseDto.IsSuccess = false;
                            break;
                        }
                    case HttpStatusCode.Forbidden:
                        {
                            _responseDto.Message = "Access Denied";
                            _responseDto.IsSuccess = false;
                            break;
                        }
                    default:
                        {
                            var content = await httpResponseMessage.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<ResponseDto>(content);
                            return result;

                        }
                }
                return _responseDto;
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
                _responseDto.IsSuccess = false;
                return _responseDto;
            }
        }
    }
}
