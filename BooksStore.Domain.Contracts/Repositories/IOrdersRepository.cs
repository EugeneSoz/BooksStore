using System.Collections.Generic;
using BooksStore.Persistence.Entities;

namespace BooksStore.Domain.Contracts.Repositories
{
    public interface IOrdersRepository
    {
        IEnumerable<OrderEntity> GetOrders();
        OrderEntity GetOrder(long key);
        OrderEntity AddOrder(OrderEntity order);
        bool UpdateOrder(OrderEntity order);
        bool DeleteOrder(OrderEntity order);
    }
}