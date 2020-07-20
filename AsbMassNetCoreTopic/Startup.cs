using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsbMassNetCoreTopic.Contracts;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AsbMassNetCoreTopic
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
            var connectionString =
        "Endpoint=sb://servicebustopicnetcore.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=blablasharedkey";

            var newPurchaseTopic = "new-purchase-topic";

            // create the bus using Azure Service bus
            var azureServiceBus = Bus.Factory.CreateUsingAzureServiceBus(busFactoryConfig =>
            {
                busFactoryConfig.Host(connectionString);

                // specify the message Purchase to be sent to a specific topic
                busFactoryConfig.Message<Purchase>(configTopology =>
                {
                    configTopology.SetEntityName(newPurchaseTopic);
                });

            });

            services.AddMassTransit
                (
                    config =>
                    {
                        config.AddBus(provider => azureServiceBus);
                    }
                );

            services.AddSingleton<IPublishEndpoint>(azureServiceBus);
            services.AddSingleton<IBus>(azureServiceBus);

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
