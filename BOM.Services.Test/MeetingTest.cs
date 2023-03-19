using BOM.Services.Api;
using BOM.Services.Api.Interfaces;
using BOM.Services.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Moq;
using System.Net.Http.Headers;
using System.Text;

namespace BOM.Services.Test
{
    [TestClass]
    public class MeetingTest : TestBase
    {
        private readonly IOpenviduApiService _openviduApiService;
        private readonly IMeetingService _service;

        public MeetingTest()
        {
            //var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //var configBuilder = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            //var configuration = configBuilder.Build();
            //var services = new ServiceCollection();
            //services.AddOptions();
            //services.Configure<AppConfig>(configuration.GetSection("AppConfig"));
            //var loggerFactory = LoggerFactory.Create(c => c.AddConsole());

            ConfigurationServer();

            var logger = CustomLoggerFactory.CreateLogger<MeetingService>();
            var openviduApiServiceLogger = CustomLoggerFactory.CreateLogger<OpenviduApiService>();
            var config = Options.Create(Configuration.GetSection("AppConfig").Get<AppConfig>());
            var openviduStoreSetting = Options.Create(Configuration.GetSection("OpenviduStoreDatabase").Get<OpenviduStoreSetting>());

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

            var _userService = new UserService(openviduStoreSetting);
            _openviduApiService = new OpenviduApiService(mockHttpClientFactory.Object, config, openviduApiServiceLogger);
            _service = new MeetingService(_openviduApiService, logger, _userService);
        }

        [TestMethod]
        public async Task ConnectToMeeting()
        {
            var sessionId = "ses_FKxT3KPEaA";
            ObjectId _id = ObjectId.GenerateNewId();
            int userId = 1;
            var token = await _service.ConnetToMeeting(_id, userId, sessionId);

            Assert.IsNotNull(token);
        }
    }
}
