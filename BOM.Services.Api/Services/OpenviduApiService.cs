using BOM.Services.Api.Interfaces;
using BOM.Services.Api.Models;
using Microsoft.Extensions.Options;
using System.Text;

namespace BOM.Services.Api.Services
{
    public class OpenviduApiService : IOpenviduApiService
    {
        private readonly HttpClient _httpClient;
        private readonly AppConfig _config;
        private readonly ILogger<OpenviduApiService> _logger;

        public OpenviduApiService(IHttpClientFactory httpClientFactory
            , IOptions<AppConfig> config
            , ILogger<OpenviduApiService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("Openvidu");
            _config = config.Value;
            _logger = logger;
        }

        public async Task<ConnectResponse> Connect(ConnectRequest request)
        {
            var result = new ConnectResponse();
            try
            {
                var content = new StringContent("", Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(string.Format(_config.OpenviduConnectionUrl, request.SessionId), content);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
                if (responseBody == null)
                {
                    _logger.LogWarning("responseBody of createSession is null");
                }
                var token = responseBody["token"].ToString().Trim('"');
                result.Token = token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "exception occoured in Connect openviduApiService");
            }
            return result;
        }

        public async Task<CreateSessionResponse> CreateSession(CreateSessionRequest request)
        {
            var result = new CreateSessionResponse();
            try
            {
                var content = new StringContent("", Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_config.OpenviduSessionUrl, content);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
                if (responseBody == null)
                {
                    _logger.LogWarning("responseBody of createSession is null");
                }
                var sessionId = responseBody["sessionId"].ToString().Trim('"');
                result.SessionId = sessionId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "exception occoured in createSession openviduApiService");
            }
            return result;
        }
    }
}
