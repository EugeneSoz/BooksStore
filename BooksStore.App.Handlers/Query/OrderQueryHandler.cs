﻿using System;
using System.Collections.Generic;
using System.Linq;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Mapping;
using BooksStore.Domain.Contracts.Models.Orders;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Repositories;
using BooksStore.Domain.Contracts.Services;
using OnlineBooksStore.App.Handlers.Interfaces;

namespace BooksStore.App.Handlers.Query
{
    public class OrderQueryHandler : 
        IQueryHandler<OrderQuery, Order>, 
        IQueryHandler<OrderIdQuery, bool>,
        IQueryHandler<PageFilterQuery, PagedList<Order>>
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IPagedListService<Order> _pagedListService;
        private readonly ISqlQueryProcessingService _sqlQueryProcessingService;

        public OrderQueryHandler(
            IOrdersRepository ordersRepository, 
            IPagedListService<Order> pagedListService,
            ISqlQueryProcessingService sqlQueryProcessingService)
        {
            _ordersRepository = ordersRepository ?? throw new ArgumentNullException(nameof(ordersRepository));
            _pagedListService = pagedListService ?? throw new ArgumentNullException(nameof(pagedListService));
            _sqlQueryProcessingService = sqlQueryProcessingService ?? throw new ArgumentNullException(nameof(sqlQueryProcessingService));
        }

        public Order Handle(OrderQuery query)
        {
            return query.MapToOrder();
        }

        public bool Handle(OrderIdQuery query)
        {
            return false;
        }

        public PagedList<Order> Handle(PageFilterQuery query)
        {
            var conditions = new QueryConditions
            {
                CurrentPage = query.CurrentPage,
                PageSize = query.PageSize
            };

            var queryCondition = _sqlQueryProcessingService.GenerateSqlQueryConditions(conditions);
            var options = query.MapToPageOptions();
            var orderEntities = _ordersRepository.GetOrders(options);
            var orders = orderEntities.orders.Select(o => o.MapToOrder()).ToList();

            var result = _pagedListService.CreatePagedList(orders, orderEntities.count, conditions);

            return result;
        }
    }
}