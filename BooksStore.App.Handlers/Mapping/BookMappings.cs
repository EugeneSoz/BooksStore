using System.Linq;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Persistence.Entities;
using OnlineBooksStore.App.Contracts.Command;

namespace BooksStore.App.Handlers.Mapping
{
    public static class BookMappings
    {
        public static BookResponse MapBookResponse(this BookEntity bookEntity)
        {
            return new BookResponse
            {
                Id = bookEntity.Id,
                Authors = bookEntity.Authors,
                BookCover = bookEntity.BookCover,
                Language = bookEntity.Language,
                Title = bookEntity.Title,
                Year = bookEntity.Year,
                CategoryId = bookEntity.CategoryId,
                PublisherId = bookEntity.PublisherId,
                Description = bookEntity.Description,
                PageCount = bookEntity.PageCount,
                PurchasePrice = bookEntity.PurchasePrice,
                RetailPrice = bookEntity.RetailPrice,
                CategoryName = bookEntity.Category.Name,
                SubcategoryName = bookEntity.Category.Name,
                PublisherName = bookEntity.Publisher.Name
            };
        }

        public static BookEntity MapBookEntity<TCommand>(this TCommand command) where TCommand : BookCommand
        {
            return new BookEntity
            {
                Id = command.Id,
                Title = command.Title,
                Authors = command.Authors,
                Year = command.Year,
                Language = command.Language,
                PageCount = command.PageCount,
                Description = command.Description,
                RetailPrice = command.RetailPrice,
                PurchasePrice = command.PurchasePrice,
                CategoryId = command.CategoryId,
                PublisherId = command.PublisherId
            };
        }
    }
}