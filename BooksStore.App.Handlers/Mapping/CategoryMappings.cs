using System.Collections.Generic;
using System.Linq;
using BooksStore.App.Contracts.Command;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Persistence.Entities;
using OnlineBooksStore.Domain.Contracts.Models.Categories;

namespace BooksStore.App.Handlers.Mapping
{
    public static class CategoryMappings
    {
        public static StoreCategoryResponse MapStoreCategoryResponse(this CategoryEntity categoryEntity)
        {
            bool hasChildren = categoryEntity.ChildrenCategories.Any();
            var storeCategory = new StoreCategoryResponse
            {
                Id = categoryEntity.Id,
                ControlId = $"c_{categoryEntity.Id}",
                Name = categoryEntity.Name,
                IsParent = hasChildren,
                HasChildren = hasChildren,
            };

            if (hasChildren)
            {
                var children = new List<StoreCategoryResponse>();
                foreach (var child in categoryEntity.ChildrenCategories)
                {
                    var storeChildCategory = new StoreCategoryResponse
                    {
                        Id = child.Id,
                        ControlId = $"c_{child.Id}",
                        ParentId = categoryEntity.Id,
                        Name = child.Name,
                        IsParent = false,
                        HasChildren = false,
                    };

                    children.Add(storeChildCategory);
                }
                storeCategory.Children = children;
            }

            return storeCategory;
        }

        public static Category MapCategory(this CategoryEntity categoryEntity)
        {
            return new Category
            {
                Id = categoryEntity.Id,
                Name = categoryEntity.Name,
                ParentId = categoryEntity.ParentId,
                ParentCategory = new Category
                {
                    Id = categoryEntity.ParentCategory.Id,
                    Name = categoryEntity.ParentCategory.Name
                },
                ChildrenCategories = categoryEntity.ChildrenCategories.Select(children => new Category
                {
                    Id = children.Id,
                    Name = children.Name,
                    ParentId = children.ParentId
                }).ToList(),
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
                ParentId = command.ParentId
            };
        }

        public static PagedList<CategoryResponse> MapCategoryResponsePagedList(this PagedList<CategoryEntity> pagedList)
        {
            return new PagedList<CategoryResponse>()
            {
                CurrentPage = pagedList.CurrentPage,
                PageSize = pagedList.PageSize,
                TotalPages = pagedList.TotalPages,
                Entities = pagedList.Entities.Select(e => new CategoryResponse
                {
                    Id = e.Id,
                    Name = e.Name,
                    ParentId = e.ParentId,
                    DisplayedName = e.ParentAndChildName,
                    ParentCategoryName = e.ParentCategory?.Name
                }).ToList()
            };
        }
    }
}