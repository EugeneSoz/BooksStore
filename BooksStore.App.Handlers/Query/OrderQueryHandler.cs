using System;
using System.Collections.Generic;
using System.Linq;
using BooksStore.App.Contracts.Query;
using BooksStore.Domain.Contracts.Repositories;
using OnlineBooksStore.App.Contracts.Query;
using OnlineBooksStore.App.Handlers.Interfaces;
using OnlineBooksStore.App.Handlers.Mapping;
using OnlineBooksStore.Domain.Contracts.Models.Orders;

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
            var orderEntities = _ordersRepository.GetOrders();
            var orders = orderEntities.Select(oe => oe.MapOrderResponse());

            return orders;
        }

        public bool Handle(OrderIdQuery query)
        {
            var orderEntity = _ordersRepository.GetOrder(query.Id);
            if (orderEntity != null)
            {
                orderEntity.Shipped = true;
                return _ordersRepository.UpdateOrder(orderEntity);
            }

            return false;
        }
    }
}