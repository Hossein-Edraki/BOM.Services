using BOM.Services.Api;
using BOM.Services.Api.Controllers;
using BOM.Services.Api.Interfaces;
using BOM.Services.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BOM.Services.Test
{
    [TestClass]
    public class JWTTest : TestBase
    {
        private readonly TokenController _controller;
        private readonly IJwtService _service;

        public JWTTest()
        {
            //var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //var configBuilder = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            //var configuration = configBuilder.Build();
            //var services = new ServiceCollection();
            //services.AddOptions();
            //services.Configure<AppConfig>(configuration.GetSection("AppConfig"));
            //var loggerFactory = LoggerFactory.Create(c => c.AddConsole());
            ConfigurationServer();

            var logger = CustomLoggerFactory.CreateLogger<JwtService>();
            var config = Options.Create(Configuration.GetSection("AppConfig").Get<AppConfig>());

            _service = new JwtService(logger, config);
            _controller = new TokenController(_service);
        }

        [TestMethod]
        public async Task TestMethodGenerateJwtTokenOK()
        {
            var userId = "1";
            var actionResult = await _controller.Get(userId);

            var okActionResult = actionResult as OkObjectResult;

            Assert.IsNotNull(okActionResult);
            Assert.IsNotNull(okActionResult.Value);
        }

        [TestMethod]
        public async Task TestMethodGenerateJwtTokenBadRequest()
        {
            var userId = "";
            var actionResult = await _controller.Get(userId);

            var badRequestActionResult = actionResult as BadRequestResult;

            Assert.IsNotNull(badRequestActionResult);
        }

        [TestMethod]
        public void TestMethodValidationJwtToken()
        {
            var userId = _service.ValidateToken("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJuYmYiOjE2NzkyMTU4MjcsImV4cCI6MTY3OTgyMDYyNywiaWF0IjoxNjc5MjE1ODI3fQ.OZ4a6QrN-_iIb33Wz23pTXbTjx1oGsWskJBURkTUci4");

            Assert.IsNotNull(userId);
            Assert.AreNotEqual(userId, 0);
        }
    }
}