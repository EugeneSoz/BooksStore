using System.Collections.Generic;

namespace BooksStore.Domain.Contracts.Models.Orders
{
    public class Order : EntityBase
    {
        public Customer Customer { get; set; }
        public bool Shipped { get; set; }
        public Payment Payment { get; set; }
        public IEnumerable<CartLine> Lines { get; set; }
    }
}