using OnlineBooksStore.Domain.Contracts.Models.Orders;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BooksStore.Domain.Contracts.Models.Orders;

namespace BooksStore.App.Contracts.Command
{
    public abstract class OrderCommand : BooksStore.App.Contracts.Command.Command
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public bool Shipped { get; set; }
        [Required]
        public IEnumerable<CartLine> Lines { get; set; }
    }

    public sealed class CreateOrderCommand : OrderCommand { }
}