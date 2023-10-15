using MassTransitDemo.Application.Entities;

namespace MassTransitDemo.Application.Interfaces.Services
{
    public interface IOrderService
    {
        public Task<int> Add(Order obj);
        public Task<int> Update(Order obj);
        public Task<IEnumerable<Order>> List();
        public Task<Order?> Get(long id);
    }
}
