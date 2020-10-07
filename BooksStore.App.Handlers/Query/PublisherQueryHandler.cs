using System;
using System.Collections.Generic;
using System.Linq;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Mapping;
using BooksStore.Domain.Contracts.Models.Pages;
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

        public PublisherQueryHandler(
            IPublishersRepository repository, 
            IPagedListService<PublisherResponse> pagedListService, 
            IPropertiesService propertiesService,
            IQueryProcessingService queryProcessingService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _pagedListService = pagedListService ?? throw new ArgumentNullException(nameof(pagedListService));
            _propertiesService = propertiesService ?? throw new ArgumentNullException(nameof(propertiesService));
            _queryProcessingService = queryProcessingService ?? throw new ArgumentNullException(nameof(queryProcessingService));
        }

        public PublishersViewModel Handle(PageConditionsQuery query)
        {
            var conditions = new QueryConditions
            {
                CurrentPage = query.CurrentPage,
                PageSize = query.PageSize,
                OrderConditions = new []{ new Condition {PropertyName = "Id", PropertyValue = "ASC" } }
            };

            var queryCondition = _queryProcessingService.GenerateSqlQueryConditions(conditions);
            var publisherEntities = _repository.GetPublishers(queryCondition);

            var publishers = publisherEntities.publishers
                .Select(pe => pe.MapPublisherResponse())
                .ToList();

            var result = _pagedListService.CreatePagedList(publishers, publisherEntities.count, conditions);

            var viewModel = new PublishersViewModel
            {
                Publishers = result.Entities,
                Pagination = result.Pagination,
                ToolbarViewModel = new AdminToolbarViewModel()
                {
                    FormUrl = string.Empty,
                    IsFormButtonVisible = true
                },
                FilterProps = _propertiesService.GetPublisherFilterProps(),
                TableHeaders = _propertiesService.GetPublisherSortingProps()
            };

            return viewModel;
        }

        public List<PublisherResponse> Handle(SearchTermQuery query)
        {
            PageOptions options = new PageOptions
            {
                SearchTerm = query.Value,
                SearchPropertyNames = new[] { nameof(Publisher.Name) },
                SortPropertyName = nameof(Publisher.Name),
                PageSize = 10
            };

            var conditions = new QueryConditions();

            var queryCondition = _queryProcessingService.GenerateSqlQueryConditions(conditions);

            var publisherEntities = _repository.GetPublishers(queryCondition);
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