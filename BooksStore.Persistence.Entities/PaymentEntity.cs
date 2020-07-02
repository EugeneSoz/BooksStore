namespace BooksStore.Persistence.Entities
{
    public class PaymentEntity : BaseEntity
    {
        public string CardNumber { get; set; }
        public string CardExpiry { get; set; }
        public int CardSecurityCode { get; set; }
        public decimal Total { get; set; }
        public string AuthCode { get; set; }
    }
}