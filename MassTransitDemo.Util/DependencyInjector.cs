using MassTransitDemo.Application.Interfaces.Services;
using MassTransitDemo.Application.Services;
using MassTransitDemo.Domain;
using MassTransitDemo.Domain.Interfaces.Repositories;
using MassTransitDemo.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransitDemo.Util
{
    public static class DependencyInjector
    {
        public static void AddDIConfiguration(this IServiceCollection services)
        {
            services.AddScoped<DatabaseSession>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
        }
    }
}