using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Mapping;
using BooksStore.Domain.Contracts.Models;
using BooksStore.Domain.Contracts.Models.Categories;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Models.Properties;
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
        private readonly IMapper _mapper;

        public CategoryQueryHandler(
            ICategoriesRepository categoriesRepository, 
            IPagedListService<CategoryResponse> pagedListService, 
            IPropertiesService propertiesService,
            IQueryProcessingService queryProcessingService,
            IMapper mapper)
        {
            _categoriesRepository = categoriesRepository ?? throw new ArgumentNullException(nameof(categoriesRepository));
            _pagedListService = pagedListService ?? throw new ArgumentNullException(nameof(pagedListService));
            _propertiesService = propertiesService ?? throw new ArgumentNullException(nameof(propertiesService));
            _queryProcessingService = queryProcessingService ?? throw new ArgumentNullException(nameof(queryProcessingService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public CategoriesViewModel Handle(PageConditionsQuery query)
        {
            var conditions = _mapper.Map<QueryConditions>(query);

            var (queryCondition, isSearchOrFilterUsed) = _queryProcessingService.GenerateSqlQueryConditions(conditions);
            var (count, categoryEntities) = _categoriesRepository.GetCategories(queryCondition, isSearchOrFilterUsed);

            var categories = categoryEntities
                .Select(ce => _mapper.Map<CategoryResponse>(ce));

            var result = _pagedListService.CreatePagedList(categories, count, conditions);

            var viewModel = new CategoriesViewModel
            {
                Entities = result.Entities,
                Pagination = result.Pagination,
                ToolbarViewModel = new AdminToolbarViewModel
                {
                    FormUrl = string.Empty,
                    IsFormButtonVisible = true
                },
                AdminFilter = new AdminFilter
                {
                    FilterProperties = _propertiesService.GetCategoryFilterProps(),
                    SelectedProperty = conditions.FilterConditions != null ? conditions.FilterConditions[0].PropertyName : string.Empty,
                    SearchValue = string.Empty,
                    Controller = "Categories",
                    Action = "ShowCategories"
                },
                TableHeaders = _propertiesService.GetCategorySortingProps(conditions),
                SortingProperty = new SortingProperty(conditions.OrderConditions[0].PropertyName, 
                    conditions.OrderConditions[0].PropertyValue)
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
            PageOptions1 options1 = new PageOptions1
            {
                SearchTerm = query.Value,
                SearchPropertyNames = new[] { nameof(CategoryEntity.Name) },
                SortPropertyName = nameof(CategoryEntity.Name),
                PageSize = 10
            };

            var conditions = new QueryConditions();

            var (queryCondition, isSearchOrFilterUsed) = _queryProcessingService.GenerateSqlQueryConditions(conditions);
            var categoryEntities = _categoriesRepository.GetCategories(queryCondition, isSearchOrFilterUsed);
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