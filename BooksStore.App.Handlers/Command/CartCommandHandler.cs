using System;
using System.Collections.Generic;
using BooksStore.App.Contracts.Command;
using BooksStore.App.Handlers.Mapping;
using BooksStore.Domain.Contracts.Models.Orders;
using BooksStore.Domain.Contracts.Repositories;
using BooksStore.Domain.Contracts.Services;
using OnlineBooksStore.App.Handlers.Interfaces;

namespace BooksStore.App.Handlers.Command
{
    public class CartCommandHandler : ICommandHandler<CartCommand, List<CartLine>>
    {
        private readonly IBooksRepository _booksRepository;
        private readonly ICartService _cartService;

        public CartCommandHandler(IBooksRepository booksRepository, ICartService cartService)
        {
            _booksRepository = booksRepository ?? throw new ArgumentNullException(nameof(booksRepository));
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        }

        public List<CartLine> Handle(CartCommand command)
        {
            var bookEntity = _booksRepository.GetBook(command.BookId);
            var book = bookEntity.MapBookResponse();
            _cartService.Lines = command.Lines;
            _cartService.AddItem(book, 1);

            return _cartService.Lines;
        }
    }
}