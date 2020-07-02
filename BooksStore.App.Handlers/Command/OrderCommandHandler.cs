using System;
using System.Collections.Generic;
using System.Linq;
using BooksStore.App.Contracts.Command;
using BooksStore.Domain.Contracts.Repositories;
using BooksStore.Persistence.Entities;
using OnlineBooksStore.App.Handlers.Interfaces;
using OnlineBooksStore.App.Handlers.Mapping;
using OnlineBooksStore.Domain.Contracts.Models.Orders;

namespace OnlineBooksStore.App.Handlers.Command
{
    public class OrderCommandHandler : ICommandHandler<CreateOrderCommand, Payment>
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IOrdersRepository _ordersRepository;

        public OrderCommandHandler(IBooksRepository booksRepository, IOrdersRepository ordersRepository)
        {
            _booksRepository = booksRepository ?? throw new ArgumentNullException(nameof(booksRepository));
            _ordersRepository = ordersRepository ?? throw new ArgumentNullException(nameof(ordersRepository));
        }

        public Payment Handle(CreateOrderCommand command)
        {
            var orderEntity = command.MapOrderEntity();
            orderEntity.Id = 0;
            orderEntity.Shipped = false;
            //не доверяем информации о сумме заказа, присланного с клиента
            orderEntity.Payment.Total = GetPrice(orderEntity.Lines);
            ProcessPayment(orderEntity.Payment);
            if (orderEntity.Payment.AuthCode != null)
            {
                var orderId = _ordersRepository.AddOrder(orderEntity).Id;
                var order = _ordersRepository.GetOrder(orderId);

                return new Payment
                {
                    AuthCode = order.Payment.AuthCode,
                    Total = order.Payment.Total
                };
            }

            return default;
        }

        private decimal GetPrice(IEnumerable<OrderLineEntity> lines)
        {
            //получить id всех книг в заказе
            var ids = lines.Select(l => l.EntityId);
            var books = _booksRepository.GetSomeBooks(ids);
            return books
                .Select(b => lines.First(l => l.EntityId == b.Id).Quantity * b.RetailPrice)
                .Sum();
        }

        private void ProcessPayment(PaymentEntity payment)
        {
            payment.AuthCode = "12345";
        }
    }
}