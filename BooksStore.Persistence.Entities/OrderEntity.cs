namespace BooksStore.Persistence.Entities
{
    public class OrderEntity : BaseEntity
    {
        public long CustomerId { get; set; }
        public bool Shipped { get; set; }
        public long PaymentId { get; set; }
    }
}