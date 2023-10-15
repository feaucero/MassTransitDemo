using MassTransit;
using MassTransitDemo.Application.Consumers;
using MassTransitDemo.Application.Models;
using MassTransitDemo.Util;

namespace MassTransitDemo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDIConfiguration();

            var rabbitMQSettings = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();

            builder.Services.AddMassTransit(x => {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMQSettings.HostName, rabbitMQSettings.VirtualHost, h => {
                        h.Username(rabbitMQSettings.UserName);
                        h.Password(rabbitMQSettings.Password);
                    });

                    cfg.ConfigureEndpoints(context);
                    cfg.PrefetchCount = 1;
                });

                x.AddConsumer<OrderInitializingEventConsumer>();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}