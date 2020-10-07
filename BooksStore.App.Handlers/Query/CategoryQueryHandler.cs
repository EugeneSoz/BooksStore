using System;
using System.Collections.Generic;
using System.Linq;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Mapping;
using BooksStore.Domain.Contracts.Models.Categories;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Repositories;
using BooksStore.Domain.Contracts.Services;
using BooksStore.Domain.Contracts.ViewModels;
using BooksStore.Persistence.Entities;
using OnlineBooksStore.App.Handlers.Interfaces;

namespace BooksStore.App.Handlers.Query
{
    public class CategoryQueryHandler : IQueryHandler<PageConditionsQuery, CategoriesViewModel>,
        IQueryHandler<EntityIdQuery, Category>,
        IQueryHandler<CategoryQuery, List<Category>>,
        IQueryHandler<SearchTermQuery, List<CategoryResponse>>,
        IQueryHandler<StoreCategoryQuery, List<StoreCategoryResponse>>
    {
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IPagedListService<CategoryResponse> _pagedListService;
        private readonly IPropertiesService _propertiesService;
        private readonly IQueryProcessingService _queryProcessingService;

        public CategoryQueryHandler(
            ICategoriesRepository categoriesRepository, 
            IPagedListService<CategoryResponse> pagedListService, 
            IPropertiesService propertiesService,
            IQueryProcessingService queryProcessingService)
        {
            _categoriesRepository = categoriesRepository ?? throw new ArgumentNullException(nameof(categoriesRepository));
            _pagedListService = pagedListService ?? throw new ArgumentNullException(nameof(pagedListService));
            _propertiesService = propertiesService ?? throw new ArgumentNullException(nameof(propertiesService));
            _queryProcessingService = queryProcessingService ?? throw new ArgumentNullException(nameof(queryProcessingService));
        }

        public CategoriesViewModel Handle(PageConditionsQuery query)
        {
            var conditions = new QueryConditions
            {
                CurrentPage = query.CurrentPage,
                PageSize = query.PageSize
            };

            var queryCondition = _queryProcessingService.GenerateSqlQueryConditions(conditions);
            var categoryEntities = _categoriesRepository.GetCategories(queryCondition);

            var categories = categoryEntities.categries
                .Select(ce => ce.MapCategoryResponse())
                .ToList();

            var result = _pagedListService.CreatePagedList(categories, categoryEntities.count, conditions);

            var viewModel = new CategoriesViewModel
            {
                Categories = result.Entities,
                Pagination = result.Pagination,
                ToolbarViewModel = new AdminToolbarViewModel
                {
                    FormUrl = string.Empty,
                    IsFormButtonVisible = true
                },
                FilterProps = _propertiesService.GetCategoryFilterProps(),
                TableHeaders = _propertiesService.GetCategorySortingProps()
            };
            return viewModel;
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
            PageOptions options = new PageOptions
            {
                SearchTerm = query.Value,
                SearchPropertyNames = new[] { nameof(CategoryEntity.Name) },
                SortPropertyName = nameof(CategoryEntity.Name),
                PageSize = 10
            };

            var conditions = new QueryConditions();

            var queryCondition = _queryProcessingService.GenerateSqlQueryConditions(conditions);
            var categoryEntities = _categoriesRepository.GetCategories(queryCondition);
            var categories = categoryEntities.categries
                .Select(ce => ce.MapCategoryResponse())
                .ToList();

            return categories;
        }

        public List<StoreCategoryResponse> Handle(StoreCategoryQuery query)
        {
            var storeCategories = _categoriesRepository.GetStoreCategories();
            var categoryResponses = storeCategories.Select(sc => sc.MapStoreCategoryResponse()).ToList();

            return categoryResponses;
        }
    }
}