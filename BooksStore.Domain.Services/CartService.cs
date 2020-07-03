using System.Collections.Generic;
using System.Linq;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Models.Orders;
using BooksStore.Domain.Contracts.Services;

namespace BooksStore.Domain.Services
{
    public class CartService : ICartService
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        public void AddItem(BookResponse book, int quantity)
        {
            var line = Lines.FirstOrDefault(p => p.BookId == book.Id);
            if (line == null) {
                Lines.Add(new CartLine 
                {
                    BookId = book.Id,
                    ItemName = book.Title,
                    Quantity = quantity,
                    Price = book.RetailPrice
                });
            } 
            else 
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(long bookId)
        {
            Lines.RemoveAll(l => l.BookId == bookId);
        }

        public decimal ComputeTotalValue()
        {
            return Lines.Sum(e => e.Price * e.Quantity);
        }

        public void Clear()
        {
            Lines.Clear();
        }
    }
}