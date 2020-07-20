using AsbMassNetCoreTopic.Consumer1.Consumers;
using AsbMassNetCoreTopic.Contracts;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AsbMassNetCoreTopic.Consumer1
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = "Endpoint=sb://servicebustopicnetcore.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=blablasharedkey";
            var newPurchaseTopic = "new-purchase-topic"; // need to make sure the topic name is written correctly
            var subscriptionName = "new-purchase-topic-subscriber_1";

            services.AddMassTransit(serviceCollectionConfigurator =>
            {
                serviceCollectionConfigurator.AddConsumer<PurchaseConsumer1>();

                //Consumers - Receivers
                //Message Creators - Senders
                //would normally be in different applications

                serviceCollectionConfigurator.AddBus
                    (registrationContext => Bus.Factory.CreateUsingAzureServiceBus
                                                    (configurator =>
                                                    {
                                                        var host = configurator.Host(connectionString);

                                                        configurator.Message<Purchase>(m => { m.SetEntityName(newPurchaseTopic); });

                                                        /*
                                                         For a consumer to receive messages, the consumer must be connected to a receive endpoint. 
                                                        This is done during bus configuration, particularly within the configuration of a receive endpoint.
                                                        https://masstransit-project.com/usage/consumers.html#consumer*/

                                                        configurator.SubscriptionEndpoint<Purchase>(subscriptionName, endpointConfigurator =>
                                                        {
                                                            endpointConfigurator.ConfigureConsumer<PurchaseConsumer1>(registrationContext);
                                                        });

                                                    }
                                                        ));

            });

            //need to always start the bus, so it behaves correctly
            services.AddSingleton<IHostedService, BusHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
