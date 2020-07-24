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
        IQueryHandler<PageFilterQuery, PublishersViewModel>,
        IQueryHandler<SearchTermQuery, List<PublisherResponse>>,
        IQueryHandler<PublisherIdQuery, Publisher>
    {
        private readonly IPublishersRepository _repository;
        private readonly IPagedListService<PublisherResponse> _pagedListService;
        private readonly IPropertiesService _propertiesService;

        public PublisherQueryHandler(
            IPublishersRepository repository, 
            IPagedListService<PublisherResponse> pagedListService, 
            IPropertiesService propertiesService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _pagedListService = pagedListService ?? throw new ArgumentNullException(nameof(pagedListService));
            _propertiesService = propertiesService ?? throw new ArgumentNullException(nameof(propertiesService));
        }

        public PublishersViewModel Handle(PageFilterQuery query)
        {
            var options = query.MapToPageOptions();
            var publisherEntities = _repository.GetPublishers(options);

            var publishers = publisherEntities.publishers
                .Select(pe => pe.MapPublisherResponse())
                .ToList();

            var result = _pagedListService.CreatePagedList(publishers, publisherEntities.count, options);

            var viewModel = new PublishersViewModel
            {
                Publishers = result.Entities,
                Pagination = result.Pagination,
                ToolbarViewModel = new AdminToolbarViewModel()
                {
                    LinkToForm = string.Empty,
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

            var publisherEntities = _repository.GetPublishers(options);
            var publishers = publisherEntities.publishers
                .Select(e => e.MapPublisherResponse())
                .ToList();

            return publishers;
        }

        public Publisher Handle(PublisherIdQuery query)
        {
            var publisherEntity = _repository.GetPublisher(query.Id);
            var publisher = publisherEntity.MapPublisher();

            return publisher;
        }
    }
}