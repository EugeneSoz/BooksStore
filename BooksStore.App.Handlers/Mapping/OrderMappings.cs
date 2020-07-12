using BooksStore.App.Contracts.Command;
using BooksStore.App.Contracts.Query;
using BooksStore.Domain.Contracts.Models.Orders;
using BooksStore.Persistence.Entities;

namespace BooksStore.App.Handlers.Mapping
{
    public static class OrderMappings
    {
        public static Order MapToOrder(this CreateOrderCommand command)
        {
            return new Order
            {
                Lines = command.Lines,
                Customer = command.Customer,
                Payment = command.Payment
            };
        }

        public static Order MapToOrder(this OrderQuery query)
        {
            return new Order
            {
                Lines = query.Lines,
                Customer = query.Customer,
                Payment = query.Payment
            };
        }

        public static Order MapToOrder(this OrderEntity order)
        {
            return new Order
            {
                Id = order.Id,
                Customer = new Customer
                {
                    Id = order.CustomerEntity.Id,
                    Name = order.CustomerEntity.Name,
                    Address = order.CustomerEntity.Address,
                    State = order.CustomerEntity.State,
                    ZipCode = order.CustomerEntity.ZipCode
                },
                Payment = new Payment
                {
                    Id = order.PaymentEntity.Id,
                    CardNumber = order.PaymentEntity.CardNumber,
                    CardExpiry = order.PaymentEntity.CardExpiry,
                    Total = order.PaymentEntity.Total,
                    CardSecurityCode = order.PaymentEntity.CardSecurityCode,
                    AuthCode = order.PaymentEntity.AuthCode
                },
                Shipped = order.Shipped
            };
        }
    }
}