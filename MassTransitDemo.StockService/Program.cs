using MassTransit;
using MassTransitDemo.Application.Consumers;
using MassTransitDemo.Application.Models;
using MassTransitDemo.Util;

namespace MassTransitDemo.StockService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var rabbitMQSettings = hostContext.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();

                    services.AddDIConfiguration();

                    services.AddMassTransit(x => {
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host(rabbitMQSettings.HostName, rabbitMQSettings.VirtualHost, h => {
                                h.Username(rabbitMQSettings.UserName);
                                h.Password(rabbitMQSettings.Password);
                            });

                            cfg.ConfigureEndpoints(context);
                            cfg.PrefetchCount = 1;
                        });

                        x.AddConsumer<StockValidatingEventConsumer>();
                    });
                })
                .Build();

            host.Run();
        }
    }
}