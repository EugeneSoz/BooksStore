using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Orders;

namespace BooksStore.Domain.Contracts.ViewModels
{
    public class CartViewModel
    {
        public string ReturnUrl { get; set; }
        public List<CartLine> Lines { get; set; }
        public decimal TotalLineSum { get; set; }
    }
}