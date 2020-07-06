namespace BooksStore.Persistence.Entities
{
    public class CartLineEntity : BaseEntity
    {
        public long BookId { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public long OrderId { get; set; }
    }
}