namespace BooksStore.Persistence.Entities
{
    public class OrderLineEntity : BaseEntity
    {
        public long EntityId { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}