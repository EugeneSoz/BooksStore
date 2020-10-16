using System.Linq;
using BooksStore.App.Contracts.Command;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Models.Publishers;
using BooksStore.Persistence.Entities;

namespace BooksStore.App.Handlers.Mapping
{
    public static class PublisherMappings
    {
        public static PublisherResponse MapPublisherResponse(this PublisherEntity publisherEntity)
        {
            return new PublisherResponse()
            {
                Id = publisherEntity.Id,
                Name = publisherEntity.Name,
                Country = publisherEntity.Country,
                Created = publisherEntity.Created
            };
        }

        public static Publisher MapPublisher(this PublisherEntity publisherEntity)
        {
            return new Publisher
            {
                Id = publisherEntity.Id,
                Name = publisherEntity.Name,
                Country = publisherEntity.Country,
                Created = publisherEntity.Created,
                Books = publisherEntity.Books.Select(b => new RelatedBook
                {
                    Id = b.Id,
                    Title = b.Title,
                    Authors = b.Authors,
                    RetailPrice = b.RetailPrice,
                    PurchasePrice = b.PurchasePrice
                }).ToList()
            };
        }

        public static PublisherEntity MapPublisherEntity<TCommand>(this TCommand command) where TCommand : PublisherCommand
        {
            return new PublisherEntity
            {
                Id = command.Id,
                Name = command.Name,
                Country = command.Country
            };
        }

        public static PagedList<PublisherResponse> MapPublisherResponsePagedList(this PagedList<PublisherEntity> pagedList)
        {
            return new PagedList<PublisherResponse>()
            {
                Entities = pagedList.Entities.Select(e => new PublisherResponse
                {
                    Id = e.Id,
                    Name = e.Name,
                    Country = e.Country
                }).ToList()
            };
        }
    }
}