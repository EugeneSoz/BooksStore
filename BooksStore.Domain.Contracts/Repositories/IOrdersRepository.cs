using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Persistence.Entities;

namespace BooksStore.Domain.Contracts.Repositories
{
    public interface IOrdersRepository
    {
        (int count, List<OrderEntity> orders) GetOrders(PageOptions1 options1);
        bool AddOrder(OrderEntity order, CustomerEntity customer, PaymentEntity payment, List<CartLineEntity> lines);
    }
}