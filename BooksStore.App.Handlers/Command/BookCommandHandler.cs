using System;
using BooksStore.App.Handlers.Mapping;
using BooksStore.Domain.Contracts.Repositories;
using BooksStore.Persistence.Entities;
using OnlineBooksStore.App.Contracts.Command;
using OnlineBooksStore.App.Handlers.Interfaces;

namespace BooksStore.App.Handlers.Command
{
    public class BookCommandHandler : ICommandHandler<CreateBookCommand, BookEntity>,
        ICommandHandler<UpdateBookCommand, bool>,
        ICommandHandler<DeleteBookCommand, bool>
    {
        private readonly IBooksRepository _booksRepository;

        public BookCommandHandler(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository ?? throw new ArgumentNullException(nameof(booksRepository));
        }

        public BookEntity Handle(CreateBookCommand command)
        {
            var book = command.MapBookEntity();
            return _booksRepository.AddBook(book);
        }

        public bool Handle(UpdateBookCommand command)
        {
            var book = command.MapBookEntity();
            return _booksRepository.UpdateBook(book);
        }

        public bool Handle(DeleteBookCommand command)
        {
            var book = command.MapBookEntity();
            return _booksRepository.Delete(book);
        }
    }
}