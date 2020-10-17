using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Mapping;
using BooksStore.Domain.Contracts.Models;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Models.Properties;
using BooksStore.Domain.Contracts.Models.Publishers;
using BooksStore.Domain.Contracts.Repositories;
using BooksStore.Domain.Contracts.Services;
using BooksStore.Domain.Contracts.ViewModels;
using OnlineBooksStore.App.Handlers.Interfaces;

namespace BooksStore.App.Handlers.Query
{
    public class PublisherQueryHandler : 
        IQueryHandler<PageConditionsQuery, PublishersViewModel>,
        IQueryHandler<SearchTermQuery, List<PublisherResponse>>,
        IQueryHandler<PublisherIdQuery, PublisherFormViewModel>
    {
        private readonly IPublishersRepository _repository;
        private readonly IPagedListService<PublisherResponse> _pagedListService;
        private readonly IPropertiesService _propertiesService;
        private readonly IQueryProcessingService _queryProcessingService;
        private readonly IMapper _mapper;

        public PublisherQueryHandler(
            IPublishersRepository repository, 
            IPagedListService<PublisherResponse> pagedListService, 
            IPropertiesService propertiesService,
            IQueryProcessingService queryProcessingService,
            IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _pagedListService = pagedListService ?? throw new ArgumentNullException(nameof(pagedListService));
            _propertiesService = propertiesService ?? throw new ArgumentNullException(nameof(propertiesService));
            _queryProcessingService = queryProcessingService ?? throw new ArgumentNullException(nameof(queryProcessingService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public PublishersViewModel Handle(PageConditionsQuery query)
        {
            var conditions = _mapper.Map<QueryConditions>(query);

            var (queryCondition, isSearchOrFilterUsed) = _queryProcessingService.GenerateSqlQueryConditions(conditions);
            var (count, publisherEntities) = _repository.GetPublishers(queryCondition, isSearchOrFilterUsed);

            var publishers = publisherEntities
                .Select(pe => _mapper.Map<PublisherResponse>(pe));

            var result = _pagedListService.CreatePagedList(publishers, count, conditions);

            var viewModel = new PublishersViewModel
            {
                Publishers = result.Entities,
                Pagination = result.Pagination,
                ToolbarViewModel = new AdminToolbarViewModel()
                {
                    FormUrl = string.Empty,
                    IsFormButtonVisible = true
                },
                AdminFilter = new AdminFilter
                {
                    FilterProperties = _propertiesService.GetPublisherFilterProps(),
                    SelectedProperty = string.Empty,
                    SearchValue = string.Empty,
                    Controller = "Publishers",
                    Action = "ShowPublishers"
                },
                TableHeaders = _propertiesService.GetPublisherSortingProps(conditions),
                SortingProperty = new SortingProperty(conditions.OrderConditions[0].PropertyName, 
                    conditions.OrderConditions[0].PropertyValue)
            };

            return viewModel;
        }

        public List<PublisherResponse> Handle(SearchTermQuery query)
        {
            PageOptions1 options1 = new PageOptions1
            {
                SearchTerm = query.Value,
                SearchPropertyNames = new[] { nameof(Publisher.Name) },
                SortPropertyName = nameof(Publisher.Name),
                PageSize = 10
            };

            var conditions = new QueryConditions();

            var (queryCondition, isSearchOrFilterUsed) = _queryProcessingService.GenerateSqlQueryConditions(conditions);

            var publisherEntities = _repository.GetPublishers(queryCondition, isSearchOrFilterUsed);
            var publishers = publisherEntities.publishers
                .Select(e => e.MapPublisherResponse())
                .ToList();

            return publishers;
        }

        public PublisherFormViewModel Handle(PublisherIdQuery query)
        {
            var publisherEntity = _repository.GetPublisher(query.Id);
            var publisher = publisherEntity.MapPublisher();

            return new PublisherFormViewModel
            {
                Publisher = publisher,
                RelatedBooks = publisher.Books,
                ToolbarViewModel = new AdminToolbarViewModel()
            };
        }
    }
}