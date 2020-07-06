using BooksStore.App.Contracts.Command;
using BooksStore.Domain.Contracts.Models.Orders;

namespace BooksStore.App.Handlers.Mapping
{
    public static class PaymentMappings
    {
        public static Payment MapToPayment(this CreatePaymentCommand command)
        {
            return new Payment
            {
                Id = command.Id,
                CardNumber = command.CardNumber,
                CardSecurityCode = command.CardSecurityCode
            };
        }
    }
}