using BOM.Services.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace BOM.Services.Test
{
    public class TestBase
    {
        protected virtual IConfiguration Configuration { get; set; }
        protected virtual ILoggerFactory CustomLoggerFactory { get; set; }

        public TestBase()
        {

        }

        protected virtual void ConfigurationServer()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configBuilder = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = configBuilder.Build();
            var services = new ServiceCollection();
            services.AddOptions();
            services.Configure<AppConfig>(Configuration.GetSection("AppConfig"));

            CustomLoggerFactory = LoggerFactory.Create(c => c.AddConsole());
        }
    }
}
