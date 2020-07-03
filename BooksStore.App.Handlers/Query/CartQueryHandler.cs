using System;
using BooksStore.App.Contracts.Query;
using BooksStore.Domain.Contracts.Services;
using BooksStore.Domain.Contracts.ViewModels;
using OnlineBooksStore.App.Handlers.Interfaces;

namespace BooksStore.App.Handlers.Query
{
    public class CartQueryHandler : IQueryHandler<CartQuery, CartViewModel>
    {
        private readonly ICartService _cartService;

        public CartQueryHandler(ICartService cartService)
        {
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        }

        public CartViewModel Handle(CartQuery query)
        {
            _cartService.Lines = query.Lines;
            return new CartViewModel
            {
                Lines = query.Lines,
                ReturnUrl = query.ReturnUrl ?? "/",
                TotalLineSum = query.Lines == null ? 0 : _cartService.ComputeTotalValue()
            };
        }
    }
}