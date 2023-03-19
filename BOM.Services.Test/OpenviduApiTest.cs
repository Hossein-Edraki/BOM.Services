using BOM.Services.Api;
using BOM.Services.Api.Interfaces;
using BOM.Services.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Http.Headers;
using System.Text;

namespace BOM.Services.Test
{

    [TestClass]
    public class OpenviduApiTest : TestBase
    {
        private readonly IOpenviduApiService _service;

        public OpenviduApiTest()
        {
            //var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //var configBuilder = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            //var configuration = configBuilder.Build();
            //var services = new ServiceCollection();
            //services.AddOptions();
            //services.Configure<AppConfig>(configuration.GetSection("AppConfig"));
            //var loggerFactory = LoggerFactory.Create(c => c.AddConsole());

            ConfigurationServer();

            var logger = CustomLoggerFactory.CreateLogger<OpenviduApiService>();
            var config = Options.Create(Configuration.GetSection("AppConfig").Get<AppConfig>());

            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(Configuration.GetValue<string>("AppConfig:OpenviduUrl"))
            };

            var token = $"{Configuration.GetValue<string>("AppConfig:OpenviduUsername")}:{Configuration.GetValue<string>("AppConfig:OpenviduSecret")}";
            var basicAuth = Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes(token));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("basic", basicAuth);
            httpClient.Timeout = TimeSpan.FromSeconds(Configuration.GetValue<int>("AppConfig:OpenviduTimeout"));

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(_ => _.CreateClient("Openvidu")).Returns(httpClient);

            _service = new OpenviduApiService(mockHttpClientFactory.Object, config, logger);
        }

        [TestMethod]
        public async Task CreateSessionOK()
        {
            var model = new Api.Models.CreateSessionRequest { };
            var result = await _service.CreateSession(model);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.SessionId);
        }

        [TestMethod]
        public async Task ConnectToSessionOK()
        {
            var sessionId = "ses_FKxT3KPEaA";
            var model = new Api.Models.ConnectRequest { SessionId = sessionId };
            var result = await _service.Connect(model);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Token);
        }
    }
}
