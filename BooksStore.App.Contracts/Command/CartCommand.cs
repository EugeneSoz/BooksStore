using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Orders;

namespace BooksStore.App.Contracts.Command
{
    public class CartCommand : Command
    {
        public long BookId { get; set; }
        public List<CartLine> Lines { get; set; }
    }
}