using System;
using BooksStore.App.Contracts.Command;
using BooksStore.App.Handlers.Mapping;
using BooksStore.Domain.Contracts.Repositories;
using BooksStore.Persistence.Entities;
using OnlineBooksStore.App.Handlers.Interfaces;

namespace BooksStore.App.Handlers.Command
{
    public class PublisherCommandHandler : ICommandHandler<CreatePublisherCommand, PublisherEntity>,
        ICommandHandler<UpdatePublisherCommand, bool>,
        ICommandHandler<DeletePublisherCommand, bool>
    {
        private readonly IPublishersRepository _repository;

        public PublisherCommandHandler(IPublishersRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public PublisherEntity Handle(CreatePublisherCommand command)
        {
            var publisher = command.MapPublisherEntity();
            return _repository.AddPublisher(publisher);
        }

        public bool Handle(UpdatePublisherCommand command)
        {
            var publisher = command.MapPublisherEntity();
            return _repository.UpdatePublisher(publisher);
        }

        public bool Handle(DeletePublisherCommand command)
        {
            var publisher = command.MapPublisherEntity();
            return _repository.DeletePublisher(publisher);
        }
    }
}