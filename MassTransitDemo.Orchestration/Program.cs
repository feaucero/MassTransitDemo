using MassTransit;
using MassTransitDemo.Application.Models;

namespace MassTransitDemo.Orchestration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    var configurationBuilder = new ConfigurationBuilder();
                    var configuration = configurationBuilder.AddEnvironmentVariables().AddJsonFile("appsettings.json")
                        .AddJsonFile($"appsettings.json")
                        .Build();

                    builder.Sources.Clear();
                    builder.AddConfiguration(configuration);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var rabbitMQSettings = hostContext.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();

                    services.AddMassTransit(x =>
                    {

                        x.AddSagaStateMachine<OrderStateMachine, OrderState>().InMemoryRepository();

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host(rabbitMQSettings.HostName, rabbitMQSettings.VirtualHost, h =>
                            {
                                h.Username(rabbitMQSettings.UserName);
                                h.Password(rabbitMQSettings.Password);
                            });

                            cfg.ConfigureEndpoints(context);
                            cfg.PrefetchCount = 1;
                        });
                    });
                })
                .Build();

            host.Run();
        }
    }
}