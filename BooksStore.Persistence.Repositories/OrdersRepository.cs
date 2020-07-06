using System;
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

        public bool AddOrder(OrderEntity order, CustomerEntity customer, PaymentEntity payment, List<CartLineEntity> lines)
        {
            const string sql = @"INSERT INTO Orders (Shipped, CustomerId, PaymentId)
                                 VALUES @shipped, @customerId, @paymentId";
            var parameters = new DynamicParameters();
            parameters.Add("@shipped", order.Shipped, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@customerId", order.CustomerId, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@paymentId", order.PaymentId, DbType.Int64, ParameterDirection.Input);
            using (var connection = _connectionProvider.OpenConnection())
            using (var transaction = connection.BeginTransaction())
            {
                var customerId = AddCustomer(connection, transaction, customer);
                var paymentId = AddPayment(connection, transaction, payment);
                var orderId = 0L;
                var linesId = 0L;
                if (customerId != 0 && paymentId != 0)
                {
                    order.CustomerId = customerId;
                    order.PaymentId = paymentId;
                    orderId = connection.QuerySingle<long>(sql, parameters, transaction);
                }

                if (orderId != 0)
                {
                    linesId = AddCartLine(connection, transaction, lines);
                }

                if (linesId > 0)
                {
                    transaction.Commit();
                    return true;
                }

                transaction.Rollback();
                return false;
            }
        }

        private long AddCustomer(IDbConnection connection, IDbTransaction transaction, CustomerEntity customer)
        {
            const string sql = @"INSERT INTO Customers (Name, Address, State, ZipCode, Created)
                                OUTPUT inserted.Id
                                VALUES @name, @address, @state, @zipCode, @created";

            var parameters = new DynamicParameters();
            parameters.Add("@name", customer.Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@address", customer.Address, DbType.String, ParameterDirection.Input);
            parameters.Add("@state", customer.State, DbType.String, ParameterDirection.Input);
            parameters.Add("@zipCode", customer.ZipCode, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@created", DateTime.Today, DbType.DateTime, ParameterDirection.Input);

            var id = connection.QuerySingle<long>(sql, parameters, transaction);

            return id;
        }

        private long AddPayment(IDbConnection connection, IDbTransaction transaction, PaymentEntity payment)
        {
            const string sql = @"INSERT INTO Customers (Name, Address, State, ZipCode, Created)
                                OUTPUT inserted.Id
                                VALUES @name, @address, @state, @zipCode, @created";

            var parameters = new DynamicParameters();
            parameters.Add("@name", payment.AuthCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@address", payment.CardExpiry, DbType.String, ParameterDirection.Input);
            parameters.Add("@created", DateTime.Today, DbType.DateTime, ParameterDirection.Input);

            var id = connection.QuerySingle<long>(sql, parameters, transaction);

            return id;
        }

        private long AddCartLine(IDbConnection connection, IDbTransaction transaction, List<CartLineEntity> lines)
        {
            const string sql = @"INSERT INTO Customers (Name, Address, State, ZipCode, Created)
                                OUTPUT inserted.Id
                                VALUES @name, @address, @state, @zipCode, @created";

            var parameters = new DynamicParameters();
            parameters.Add("@created", DateTime.Today, DbType.DateTime, ParameterDirection.Input);

            var id = connection.QuerySingle<long>(sql, parameters, transaction);

            return id;
        }
    }
}