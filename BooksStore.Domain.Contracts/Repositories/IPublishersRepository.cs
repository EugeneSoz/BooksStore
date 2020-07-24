using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Persistence.Entities;

namespace BooksStore.Domain.Contracts.Repositories
{
    public interface IPublishersRepository
    {
        (int count, List<PublisherEntity> publishers) GetPublishers(PageOptions options);
        PublisherEntity GetPublisher(long id);
        PublisherEntity AddPublisher(PublisherEntity publisher);
        bool UpdatePublisher(PublisherEntity publisher);
        bool DeletePublisher(PublisherEntity publisher);
    }
}