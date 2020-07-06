using System.Collections.Generic;
using System.Data;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Repositories;
using BooksStore.Persistence.Entities;
using BooksStore.Persistence.Repositories.Providers;
using Dapper;

namespace BooksStore.Persistence.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly ConnectionProvider _connectionProvider;

        public OrdersRepository(ConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public (int, List<OrderEntity>) GetOrders(PageOptions options)
        {
            if (string.IsNullOrEmpty(options.SortPropertyName))
            {
                options.SortPropertyName = nameof(PublisherEntity.Name);
                options.DescendingOrder = false;
            }
            const string rowsCountSql = @"SELECT COUNT(*) AS [Count]
                                   FROM Orders";

            var queryProcessing = new QueryProcessing<PageOptions>(options);
            var sql = $@"SELECT *
                                FROM Orders {queryProcessing.GetQueryConditions()}";
            using (var connection = _connectionProvider.OpenConnection())
            {
                var rowsCount = connection.ExecuteScalar<int>(rowsCountSql);
                var orders = connection.Query<OrderEntity>(sql);

                return (0, new List<OrderEntity>());
            }
        }

        public OrderEntity AddOrder(OrderEntity order)
        {
            const string sql = @"INSERT INTO Orders (Shipped, CustomerId, PaymentId)
                                 VALUES @shipped, @customerId, @paymentId";
            var parameters = new DynamicParameters();
            parameters.Add("@shipped", order.Shipped, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@customerId", order.CustomerId, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@paymentId", order.PaymentId, DbType.Int64, ParameterDirection.Input);
            using (var connection = _connectionProvider.OpenConnection())
            {
                var affectedRows = connection.Execute(sql, parameters);

                return affectedRows > 0 ? new OrderEntity() : null;
            }
        }
    }
}