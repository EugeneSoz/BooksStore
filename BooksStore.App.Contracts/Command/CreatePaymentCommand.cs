using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BooksStore.Domain.Contracts.Models.Orders;

namespace BooksStore.App.Contracts.Command
{
    public class CreatePaymentCommand : Command
    {
        [Required(ErrorMessage = "Укажите номер банковской карты")]
        public string CardNumber { get; set; }
        [Required(ErrorMessage = "Укажите срок окончания банковской карты")]
        public string CardExpiry { get; set; }
        [Required(ErrorMessage = "Укажите код безопасноти банковской карты")]
        public int CardSecurityCode { get; set; }
        public List<CartLine> Lines { get; set; }
    }
}