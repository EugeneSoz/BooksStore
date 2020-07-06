using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Orders;

namespace BooksStore.App.Contracts.Query
{
    public class OrderQuery : Query
    {
        public Customer Customer { get; set; }
        public Payment Payment { get; set; }
        public List<CartLine> Lines { get; set; }
    }
}