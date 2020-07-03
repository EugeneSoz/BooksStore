using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Models.Orders;

namespace BooksStore.Domain.Contracts.Services
{
    public interface ICartService
    {
        List<CartLine> Lines { get; set; }
        void AddItem(BookResponse book, int quantity);
        void RemoveLine(long bookId);
        decimal ComputeTotalValue();
        void Clear();
    }
}