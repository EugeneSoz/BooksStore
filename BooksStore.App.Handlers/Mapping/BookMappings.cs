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
                CategoryName = bookEntity.Category.ParentCategory == null
                    ? string.Empty
                    : bookEntity.Category.ParentCategory.Name,
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

        public static PagedList<BookResponse> MapBookResponsePagedList(this PagedList<BookEntity> pagedList)
        {
            return new PagedList<BookResponse>()
            {
                CurrentPage = pagedList.CurrentPage,
                PageSize = pagedList.PageSize,
                TotalPages = pagedList.TotalPages,
                Entities = pagedList.Entities.Select(e => new BookResponse
                {
                    Id = e.Id,
                    Authors = e.Authors,
                    BookCover = e.BookCover,
                    Language = e.Language,
                    Title = e.Title,
                    Year = e.Year,
                    CategoryId = e.CategoryId,
                    PublisherId = e.PublisherId,
                    Description = e.Description,
                    PageCount = e.PageCount,
                    PurchasePrice = e.PurchasePrice,
                    RetailPrice = e.RetailPrice,
                    CategoryName = e.Category.ParentCategory == null 
                        ? string.Empty 
                        : e.Category.ParentCategory.Name,
                    SubcategoryName = e.Category.Name,
                    PublisherName = e.Publisher.Name
                }).ToList()
            };
        }
    }
}