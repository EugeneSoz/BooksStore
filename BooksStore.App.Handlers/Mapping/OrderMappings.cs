using BooksStore.App.Contracts.Command;
using BooksStore.App.Contracts.Query;
using BooksStore.Domain.Contracts.Models.Orders;

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
    }
}