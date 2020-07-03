using System;
using System.Collections.Generic;
using BooksStore.App.Client.Infrastructure;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Query;
using BooksStore.Domain.Contracts.Models.Orders;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.App.Client.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly CartQueryHandler _cartQueryHandler;

        public CartSummaryViewComponent(CartQueryHandler cartQueryHandler)
        {
            _cartQueryHandler = cartQueryHandler ?? throw new ArgumentNullException(nameof(cartQueryHandler));
        }

        public IViewComponentResult Invoke()
        {
            var lines = HttpContext.Session.GetJson<List<CartLine>>("cart");
            var query = new CartQuery
            {
                Lines = lines, ReturnUrl = ViewContext.HttpContext.Request.PathAndQuery()
            };
            var result = _cartQueryHandler.Handle(query);

            return View(result);
        }
    }
}