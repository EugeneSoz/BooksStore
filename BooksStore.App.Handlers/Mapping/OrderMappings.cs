using System.Collections.Generic;
using System.Linq;
using BooksStore.App.Contracts.Command;
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
    }
}