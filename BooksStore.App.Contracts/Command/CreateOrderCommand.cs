using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Orders;

namespace BooksStore.App.Contracts.Command
{
    public class CreateOrderCommand : Command
    {
        public int Confirmed { get; set; }
        public Customer Customer { get; set; }
        public Payment Payment { get; set; }
        public List<CartLine> Lines { get; set; }
    }
}