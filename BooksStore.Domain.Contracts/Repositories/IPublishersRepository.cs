﻿using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Persistence.Entities;

namespace BooksStore.Domain.Contracts.Repositories
{
    public interface IPublishersRepository
    {
        (int count, IEnumerable<PublisherEntity> publishers) GetPublishers(SqlQueryConditions sqlQueryConditions);
        PublisherEntity GetPublisher(long id);
        PublisherEntity AddPublisher(PublisherEntity publisher);
        bool UpdatePublisher(PublisherEntity publisher);
        bool DeletePublisher(PublisherEntity publisher);
    }
}