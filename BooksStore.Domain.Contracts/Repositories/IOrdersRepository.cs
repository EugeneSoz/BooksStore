using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Persistence.Entities;

namespace BooksStore.Domain.Contracts.Repositories
{
    public interface IOrdersRepository
    {
        (int, List<OrderEntity>) GetOrders(PageOptions options);
        OrderEntity AddOrder(OrderEntity order);
    }
}