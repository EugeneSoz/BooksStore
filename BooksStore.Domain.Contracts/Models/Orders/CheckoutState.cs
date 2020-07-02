namespace OnlineBooksStore.Domain.Contracts.Models.Orders
{
    public class CheckoutState
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string CardNumber { get; set; }
        public string CardExpiry { get; set; }
        public string CardSecurityCode { get; set; }
    }
}
