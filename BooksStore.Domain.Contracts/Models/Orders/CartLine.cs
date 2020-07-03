namespace BooksStore.Domain.Contracts.Models.Orders
{
    public class CartLine : EntityBase
    {
        public long BookId { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}