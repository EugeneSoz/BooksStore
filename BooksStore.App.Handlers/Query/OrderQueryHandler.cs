using System;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Mapping;
using BooksStore.Domain.Contracts.Models.Orders;
using BooksStore.Domain.Contracts.Repositories;
using OnlineBooksStore.App.Handlers.Interfaces;

namespace BooksStore.App.Handlers.Query
{
    public class OrderQueryHandler : 
        IQueryHandler<OrderQuery, Order>, 
        IQueryHandler<OrderIdQuery, bool>
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrderQueryHandler(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository ?? throw new ArgumentNullException(nameof(ordersRepository));
        }

        public Order Handle(OrderQuery query)
        {
            return query.MapToOrder();
        }

        public bool Handle(OrderIdQuery query)
        {
            return false;
        }
    }
}