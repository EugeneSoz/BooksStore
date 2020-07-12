namespace BooksStore.Persistence.Entities
{
    public class OrderEntity : BaseEntity
    {
        public long CustomerId { get; set; }
        public bool Shipped { get; set; }
        public long PaymentId { get; set; }
        public CustomerEntity CustomerEntity { get; set; }
        public PaymentEntity PaymentEntity { get; set; }
    }
}