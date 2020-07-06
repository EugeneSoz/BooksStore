using BooksStore.App.Contracts.Command;
using BooksStore.Domain.Contracts.Models.Orders;

namespace BooksStore.App.Handlers.Mapping
{
    public static class CustomerMappings
    {
        public static Customer MapToCustomer(this CreateCustomerCommand command)
        {
            return new Customer
            {
                Id = command.Id,
                Name = command.Name,
                Address = command.Address,
                State = command.State,
                ZipCode = command.ZipCode
            };
        }
    }
}