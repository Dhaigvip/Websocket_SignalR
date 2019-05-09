using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Unity;
using Unity.Microsoft.DependencyInjection;

namespace TCM.Web.API.Signalr.Core
{
    public class Program
    {
        private static readonly IUnityContainer _container = new UnityContainer();
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(true)
                .UseUnityServiceProvider(_container)
                //.UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    var builtConfig = config.Build();
                    var configurationBuilder = new ConfigurationBuilder();
                    configurationBuilder.SetBasePath(builderContext.HostingEnvironment.ContentRootPath);
                    configurationBuilder.AddJsonFile($"appsettings.{builderContext.HostingEnvironment.EnvironmentName}.json", true, true);

                    configurationBuilder.AddEnvironmentVariables();
                    config.AddConfiguration(configurationBuilder.Build());
                })
                .UseStartup<Startup>();
        }
    }
}
