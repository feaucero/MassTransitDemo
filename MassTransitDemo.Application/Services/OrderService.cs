using MassTransitDemo.Application.Entities;
using MassTransitDemo.Application.Interfaces.Services;
using MassTransitDemo.Domain.Interfaces.Repositories;

namespace MassTransitDemo.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository= orderRepository;
        }

        public async Task<int> Add(Order obj)
        {
            return await _orderRepository.Add(obj);
        }

        public async Task<Order?> Get(long id)
        {
            return await _orderRepository.Get(id);
        }

        public async Task<IEnumerable<Order>> List()
        {
            return await _orderRepository.List();
        }

        public async Task<int> Update(Order obj)
        {
            return await _orderRepository.Update(obj);
        }
    }
}
