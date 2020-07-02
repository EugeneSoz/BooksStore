using System.Collections.Generic;

namespace BooksStore.Persistence.Entities
{
    public class OrderEntity : BaseEntity
    {
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public bool Shipped { get; set; }
        public long PaynentId { get; set; }
        public PaymentEntity Payment { get; set; }
        public IEnumerable<OrderLineEntity> Lines { get; set; }
    }
}