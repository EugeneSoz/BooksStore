using System;
using System.Collections.Generic;
using System.Linq;
using BooksStore.App.Contracts.Command;
using BooksStore.App.Handlers.Mapping;
using BooksStore.Domain.Contracts.Models.Orders;
using BooksStore.Domain.Contracts.Repositories;
using BooksStore.Persistence.Entities;
using OnlineBooksStore.App.Handlers.Interfaces;

namespace BooksStore.App.Handlers.Command
{
    public class OrderCommandHandler : 
        ICommandHandler<CreateCustomerCommand, Customer>,
        ICommandHandler<CreatePaymentCommand, Payment>,
        ICommandHandler<CreateOrderCommand, OrderEntity>
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IOrdersRepository _ordersRepository;

        public OrderCommandHandler(IBooksRepository booksRepository, IOrdersRepository ordersRepository)
        {
            _booksRepository = booksRepository ?? throw new ArgumentNullException(nameof(booksRepository));
            _ordersRepository = ordersRepository ?? throw new ArgumentNullException(nameof(ordersRepository));
        }

        public Customer Handle(CreateCustomerCommand command)
        {
            return command.MapToCustomer();
        }

        public Payment Handle(CreatePaymentCommand command)
        {
            var payment = command.MapToPayment();
            payment.Total = GetTotalPrice(command.Lines);
            payment.AuthCode = ProcessPayment(payment);

            return payment;
        }

        public OrderEntity Handle(CreateOrderCommand command)
        {
            var order = command.MapToOrder();
            var result = _ordersRepository.AddOrder(new OrderEntity());

            return result;
        }

        private decimal GetTotalPrice(List<CartLine> lines)
        {
            //получить id всех книг в заказе
            var ids = lines.Select(l => l.BookId);
            var books = _booksRepository.GetBooksByIds(ids);

            return books
                .Select(b => lines.First(l => l.BookId == b.Id).Quantity * b.RetailPrice)
                .Sum();
        }

        private string ProcessPayment(Payment payment)
        {
            return "ad" + payment.GetHashCode();
        }
    }
}