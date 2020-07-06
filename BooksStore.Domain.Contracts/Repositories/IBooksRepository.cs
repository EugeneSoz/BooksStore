using System.Collections.Generic;
using System.Threading;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Persistence.Entities;

namespace BooksStore.Domain.Contracts.Repositories
{
    public interface IBooksRepository
    {
        (int count, List<BookEntity> books) GetBooks(PageOptions options);
        IEnumerable<BookEntity> GetBooksByIds(IEnumerable<long> booksIds);
        BookEntity GetBook(long key);
        BookEntity AddBook(BookEntity book);
        bool UpdateBook(BookEntity book);
        bool Delete(BookEntity book);
    }
}
