using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Orders;

namespace BooksStore.App.Contracts.Query
{
    public class CartQuery : Query
    {
        public string ReturnUrl { get; set; }
        public List<CartLine> Lines { get; set; }
    }
}