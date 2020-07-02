using System;
using System.Collections.Generic;
using System.Linq;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Mapping;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Repositories;
using BooksStore.Persistence.Entities;
using OnlineBooksStore.App.Contracts.Query;
using OnlineBooksStore.App.Handlers.Interfaces;
using OnlineBooksStore.App.Handlers.Mapping;
using OnlineBooksStore.Domain.Contracts.Models.Categories;

namespace BooksStore.App.Handlers.Query
{
    public class CategoryQueryHandler : IQueryHandler<PageFilterQuery, PagedResponse<CategoryResponse>>,
        IQueryHandler<EntityIdQuery, Category>,
        IQueryHandler<CategoryQuery, List<Category>>,
        IQueryHandler<SearchTermQuery, List<CategoryResponse>>,
        IQueryHandler<StoreCategoryQuery, List<StoreCategoryResponse>>
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public CategoryQueryHandler(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository ?? throw new ArgumentNullException(nameof(categoriesRepository));
        }

        public PagedResponse<CategoryResponse> Handle(PageFilterQuery query)
        {
            var options = query.MapQueryOptions();
            var categoryEntities = _categoriesRepository.GetCategories(options);

            var categoriesPagedList = categoryEntities.MapCategoryResponsePagedList();
            var result = categoriesPagedList.MapPagedResponse();

            return result;
        }

        public Category Handle(EntityIdQuery query)
        {
            var categoryEntity = _categoriesRepository.GetCategory(query.Id);
            var category = categoryEntity.MapCategory();

            return category;
        }

        public List<Category> Handle(CategoryQuery query)
        {
            var categoryEntities = _categoriesRepository.GetParentCategories();
            var categories = categoryEntities.Select(ce => ce.MapCategory()).ToList();

            return categories;
        }

        public List<CategoryResponse> Handle(SearchTermQuery query)
        {
            QueryOptions options = new QueryOptions
            {
                SearchTerm = query.Value,
                SearchPropertyNames = new[] { nameof(CategoryEntity.Name) },
                SortPropertyName = nameof(CategoryEntity.Name),
                PageSize = 10
            };

            var categoryEntities = _categoriesRepository.GetCategories(options);
            var categoriesPagedList = categoryEntities.MapCategoryResponsePagedList();
            var result = categoriesPagedList.MapPagedResponse();

            return result.Entities;
        }

        public List<StoreCategoryResponse> Handle(StoreCategoryQuery query)
        {
            var storeCategories = _categoriesRepository.GetStoreCategories();
            var categoryResponses = storeCategories.Select(sc => sc.MapStoreCategoryResponse()).ToList();

            return categoryResponses;
        }
    }
}