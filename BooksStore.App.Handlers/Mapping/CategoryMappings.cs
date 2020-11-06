using System.Linq;
using BooksStore.App.Contracts.Command;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Models.Categories;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Persistence.Entities;

namespace BooksStore.App.Handlers.Mapping
{
    public static class CategoryMappings
    {
        public static StoreCategoryResponse MapStoreCategoryResponse(this CategoryEntity categoryEntity)
        {
            bool hasChildren = false;
            var storeCategory = new StoreCategoryResponse
            {
                Id = categoryEntity.Id,
                ControlId = $"c_{categoryEntity.Id}",
                Name = categoryEntity.Name,
                IsParent = hasChildren,
                HasChildren = hasChildren,
            };

            return storeCategory;
        }

        public static Category MapCategory(this CategoryEntity categoryEntity)
        {
            return new Category
            {
                Id = categoryEntity.Id,
                Name = categoryEntity.Name,
                Books = categoryEntity.Books.Select(b => new RelatedBook
                {
                    Id = b.Id,
                    Title = b.Title,
                    Authors = b.Authors,
                    RetailPrice = b.RetailPrice,
                    PurchasePrice = b.PurchasePrice
                }).ToList()
            };
        }

        public static CategoryEntity MapCategoryEntity<TCommand>(this TCommand command) where TCommand : CategoryCommand
        {
            return new CategoryEntity
            {
                Id = command.Id,
                Name = command.Name,
            };
        }

        public static CategoryResponse MapCategoryResponse(this CategoryEntity categoryEntity)
        {
            return new CategoryResponse
            {
                Id = categoryEntity.Id,
                Name = categoryEntity.Name,
            };
        }
    }
}