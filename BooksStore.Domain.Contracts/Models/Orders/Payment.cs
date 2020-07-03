using BooksStore.Domain.Contracts.Models;

namespace OnlineBooksStore.Domain.Contracts.Models.Orders
{
    public class Payment : EntityBase
    {
        public string CardNumber { get; set; }
        public string CardExpiry { get; set; }
        public int CardSecurityCode { get; set; }
        public decimal Total { get; set; }
        public string AuthCode { get; set; }
    }
}