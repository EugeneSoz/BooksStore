using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Persistence.Entities;

namespace BooksStore.Domain.Contracts.Repositories
{
    public interface IPublishersRepository
    {
        PagedList<PublisherEntity> GetPublishers(QueryOptions options);
        PublisherEntity GetPublisher(long id);
        PublisherEntity AddPublisher(PublisherEntity publisher);
        bool UpdatePublisher(PublisherEntity publisher);
        bool DeletePublisher(PublisherEntity publisher);
    }
}