using Dapper;
using MassTransitDemo.Application.Entities;
using MassTransitDemo.Domain.Interfaces.Repositories;
using System.Globalization;
using System.Xml.Linq;

namespace MassTransitDemo.Domain.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DatabaseSession _session;

        private readonly string _sqlSelectOrder = @"
SELECT
Id,
Code,
ProductId,
OrderStatus,
CreatedAt,
StockValidatedAt,
PaymentValidatedAt,
ConfirmedAt,
ClosedAt
FROM
    dbo.[Order]
/**where**/";

        private readonly string _sqlUpdateOrder = @"
UPDATE
dbo.[Order]
/**set**/
/**where**/
";

        private readonly string _sqlInsertOrder = @"
INSERT INTO dbo.[Order]
(
    OrderStatus,
    Code,
    ProductId,
    CreatedAt,
    StockValidatedAt,
    PaymentValidatedAt,
    ConfirmedAt,
    ClosedAt
)
OUTPUT INSERTED.[Id]
VALUES
(
    @OrderStatus,
    @Code,
    @ProductId,
    @CreatedAt,
    @StockValidatedAt,
    @PaymentValidatedAt,
    @ConfirmedAt,
    @ClosedAt
)
";

        public OrderRepository(DatabaseSession session)
        {
            _session = session;
        }

        public async Task<int> Add(Order order)
        {
            var transaction = _session.Connection.BeginTransaction();
            try
            {
                var insertedId = _session.Connection.QuerySingle<int>(_sqlInsertOrder, order, transaction);
                transaction.Commit();
                return insertedId;
            }
            catch (System.Exception ex)
            {
                transaction.Rollback();
                return 0;
            }
        }

        public async Task<Order?> Get(long id)
        {
            var builder = new SqlBuilder();
            var selector = builder.AddTemplate(_sqlSelectOrder);

            builder.Where("Id = @Id", new { Id = id });

            var orders = await _session.Connection.QueryAsync<Order>(selector.RawSql, selector.Parameters, _session.Transaction);

            if (orders.Any())
            {
                return orders.FirstOrDefault();
            }

            return null;
        }

        public async Task<IEnumerable<Order>> List()
        {
            var builder = new SqlBuilder();
            var selector = builder.AddTemplate(_sqlSelectOrder);

            return await _session.Connection.QueryAsync<Order>(selector.RawSql, selector.Parameters, _session.Transaction);
        }

        public async Task<int> Update(Order order)
        {
            var builder = new SqlBuilder();
            var selector = builder.AddTemplate(_sqlUpdateOrder);

            builder.Where("Code = @Code", new { order.Code });

            if (order.CreatedAt.HasValue)
            {
                builder.Set("CreatedAt = @CreatedAt", new { order.CreatedAt });
            }

            if (order.StockValidatedAt.HasValue)
            {
                builder.Set("StockValidatedAt = @StockValidatedAt", new { order.StockValidatedAt });
            }

            if (order.PaymentValidatedAt.HasValue)
            {
                builder.Set("PaymentValidatedAt = @PaymentValidatedAt", new { order.PaymentValidatedAt });
            }

            if (order.ConfirmedAt.HasValue)
            {
                builder.Set("ConfirmedAt = @ConfirmedAt", new { order.ConfirmedAt });
            }

            if (order.ClosedAt.HasValue)
            {
                builder.Set("ClosedAt = @ClosedAt", new { order.ClosedAt });
            }

            builder.Set("OrderStatus = @OrderStatus", new { order.OrderStatus });

            return await _session.Connection.ExecuteAsync(selector.RawSql, selector.Parameters, _session.Transaction);
        }
    }
}
