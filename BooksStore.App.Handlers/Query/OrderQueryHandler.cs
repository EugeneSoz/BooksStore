using System;
using System.Collections.Generic;
using System.Linq;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Mapping;
using BooksStore.Domain.Contracts.Models.Orders;
using BooksStore.Domain.Contracts.Repositories;
using OnlineBooksStore.App.Handlers.Interfaces;
using OnlineBooksStore.App.Handlers.Mapping;

namespace BooksStore.App.Handlers.Query
{
    public class OrderQueryHandler : 
        IQueryHandler<OrderQuery, IEnumerable<Order>>, 
        IQueryHandler<OrderIdQuery, bool>
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrderQueryHandler(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository ?? throw new ArgumentNullException(nameof(ordersRepository));
        }

        public IEnumerable<Order> Handle(OrderQuery query)
        {
            //var orderEntities = _ordersRepository.GetOrders();
            IEnumerable<Order> orders = null;//orderEntities.Select(oe => oe.MapOrderResponse());

            return orders;
        }

        public bool Handle(OrderIdQuery query)
        {
            return false;
        }
    }
}