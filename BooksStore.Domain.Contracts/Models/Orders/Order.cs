using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models;
using BooksStore.Domain.Contracts.Models.Orders;

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
        public IEnumerable<CartLine> Lines { get; set; }
    }
}