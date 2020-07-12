using System;
using System.Collections.Generic;
using System.Linq;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Mapping;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Models.Publishers;
using BooksStore.Domain.Contracts.Repositories;
using OnlineBooksStore.App.Handlers.Interfaces;
using OnlineBooksStore.Domain.Contracts.Models.Publishers;

namespace BooksStore.App.Handlers.Query
{
    public class PublisherQueryHandler : 
        IQueryHandler<PageFilterQuery, PagedList<PublisherResponse>>,
        IQueryHandler<SearchTermQuery, List<PublisherResponse>>,
        IQueryHandler<PublisherIdQuery, Publisher>
    {
        private readonly IPublishersRepository _repository;

        public PublisherQueryHandler(IPublishersRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public PagedList<PublisherResponse> Handle(PageFilterQuery query)
        {
            var options = query.MapToPageOptions();
            var publisherEntities = _repository.GetPublishers(options);

            var publishersPagedList = publisherEntities.MapPublisherResponsePagedList();

            return publishersPagedList;
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
            var publishers = publisherEntities.Entities
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