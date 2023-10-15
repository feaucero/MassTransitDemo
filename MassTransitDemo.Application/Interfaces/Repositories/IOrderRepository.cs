using MassTransitDemo.Application.Entities;

namespace MassTransitDemo.Domain.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        public Task<int> Add(Order obj);
        public Task<int> Update(Order obj);
        public Task<IEnumerable<Order>> List();
        public Task<Order?> Get(long id);
    }
}
