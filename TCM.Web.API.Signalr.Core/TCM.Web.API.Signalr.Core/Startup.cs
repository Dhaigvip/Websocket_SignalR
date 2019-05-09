using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonCore.Tcm.Common.Tcm.Logger;
using CommonCore.Tcm.Common.UnityContainer;
using FlightControlCore.Factory;
using FlightControlWindows.Factory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using FlightControlCore.impl;
using Unity;
using TCM.Web.API.Core.Signalr;
using Newtonsoft.Json.Serialization;
using System.Configuration;
using FlightControlCore.Endpoint.WebsphereMQ;
using Newtonsoft.Json;

namespace TCM.Web.API.Signalr.Core
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var allowedOrigins = Configuration.GetSection("AllowedOrigins").Get<List<string>>();


            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins(allowedOrigins.ToArray()));
            });
            services.AddMvc()
                .AddJsonOptions(options =>
                  {
                      options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                      options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                  })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSignalR(options =>
            {

                options.EnableDetailedErrors = true;
            }).AddJsonProtocol(options =>
            {
                options.PayloadSerializerSettings.ContractResolver = new DefaultContractResolver();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("CorsPolicy");
            app.UseMvc();
            app.UseSignalR(routes =>
            {
                routes.MapHub<TestHub>("/hub");
                routes.MapHub<DashboardQueryHub>("/dashboard");
            });
        }

        public void ConfigureContainer(IUnityContainer container)
        {
            UnityHelper.Container =
                UnityContainerSetUp.Configure(container, "Common");

            var log = container.Resolve<ILogger>();

            var DashboardSettings = Configuration.GetSection("DashboardSettings").Get<DashboardSettings>();


            var fcFactory = new FlightControlFactory(container);
            fcFactory.Defaults.NameEditor = s => s;

            if (!DashboardSettings.UseWebSpear)
            {
                fcFactory
               .AddConsumer<DashboarQueryConsumer>("DashboarQueryConsumer")
               .AddMsmqEndPoint(DashboardSettings.DashboardInternalQueue)
               .AddMsMqDeadLetter(DashboardSettings.DeadletterQueue)
               .AddMsmqEndPoint(DashboardSettings.DashboardgatewayQueue);
            }
            else
            {
                //Get webspear settings..
                //var t = ConfigurationManager.GetSection("MqSettings");
                var t = Configuration.GetSection("MqSettings").Get<MyMqSettings>();

                var str = JsonConvert.SerializeObject(t);
                var _mqSettings = JsonConvert.DeserializeObject<MqSettings>(str);

                //var _mqSettings = (MqSettings)t;
                if (_mqSettings == null)
                {
                    throw new ConfigurationErrorsException("WebSphereMQ section not found in config file.");
                }

                fcFactory
                .AddConsumer<DashboarQueryConsumer>("DashboarQueryConsumer")
                .AddMsmqEndPoint(DashboardSettings.DashboardInternalQueue)
                .AddWebsphereMqEndPoint(DashboardSettings.WebspearReadQueue, _mqSettings.Connections[0], FlightControlFactory.MockUp.No)
                .AddWebsphereMqEndPoint(DashboardSettings.DashboardgatewayQueue, _mqSettings.Connections[1], FlightControlFactory.MockUp.No, FlightControlFactory.MessageReader.None)
                .AddMsMqDeadLetter(DashboardSettings.DeadletterQueue);
            }
            

            var flightControl = fcFactory.CreateFlightControl();
            flightControl.
                ReplaceTypeName("DashboardMessage.DashboardData, DashboardMessage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
               .With(typeof(DashboardMessage.DashboardData));

            flightControl.IsLogIncommingMessages = true;

        }
    }
}
