namespace OnlineBooksStore.Domain.Contracts.Models.Orders
{
    public class OrderLine : EntityBase
    {
        public long EntityId { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}