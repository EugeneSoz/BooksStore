using System.Collections.Generic;
using System.Linq;
using BooksStore.App.Contracts.Command;
using BooksStore.Persistence.Entities;
using OnlineBooksStore.Domain.Contracts.Models.Orders;

namespace OnlineBooksStore.App.Handlers.Mapping
{
    public static class OrderMappings
    {
        public static Order MapOrderResponse(this OrderEntity entity)
        {
            var lines = new List<OrderLine>();
            entity.Lines.ToList().ForEach(l =>
            {
                lines.Add(new OrderLine
                {
                    Id = l.Id,
                    EntityId = l.EntityId,
                    ItemName = l.ItemName,
                    Quantity = l.Quantity,
                    Price = l.Price
                });
            });
            return new Order
            {
                Id = entity.Id,
                Address = entity.Address,
                State = entity.State,
                ZipCode = entity.ZipCode,
                Shipped = entity.Shipped,
                Paynent = new Payment
                {
                    Id = entity.PaynentId,
                    AuthCode = entity.Payment.AuthCode,
                    CardExpiry = entity.Payment.CardExpiry,
                    CardNumber = entity.Payment.CardNumber,
                    CardSecurityCode = entity.Payment.CardSecurityCode,
                    Total = entity.Payment.Total
                },
                CustomerName = entity.CustomerName,
                Lines = lines.AsEnumerable()
            };
        }

        public static OrderEntity MapOrderEntity<TCommand>(this TCommand command) where TCommand : OrderCommand
        {
            var lines = new List<OrderLineEntity>();
            command.Lines.ToList().ForEach(l =>
            {
                lines.Add(new OrderLineEntity
                {
                    Id = l.Id,
                    ItemName = l.ItemName,
                    EntityId = l.EntityId,
                    Quantity = l.Quantity,
                    Price = l.Price
                });
            });
            return new OrderEntity
            {
                Id = command.Id,
                Address = command.Address,
                Shipped = command.Shipped,
                Payment = new PaymentEntity(),
                CustomerName = command.Name,
                Lines = lines.AsEnumerable()
            };
        }
    }
}