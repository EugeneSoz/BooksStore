using System.Collections.Generic;

namespace OnlineBooksStore.Domain.Contracts.Models.Orders
{
    public class Order : EntityBase
    {
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public bool Shipped { get; set; }
        public Payment Paynent { get; set; }
        public IEnumerable<OrderLine> Lines { get; set; }
    }
}