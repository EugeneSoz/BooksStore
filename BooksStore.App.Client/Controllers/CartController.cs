using System;
using System.Collections.Generic;
using BooksStore.App.Client.Infrastructure;
using BooksStore.App.Contracts.Command;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Command;
using BooksStore.App.Handlers.Query;
using BooksStore.Domain.Contracts.Models.Orders;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.App.Client.Controllers
{
    public class CartController : Controller
    {
        private readonly CartQueryHandler _cartQueryHandler;
        private readonly CartCommandHandler _cartCommandHandler;

        public CartController(CartQueryHandler cartQueryHandler, CartCommandHandler cartCommandHandler)
        {
            _cartQueryHandler = cartQueryHandler ?? throw new ArgumentNullException(nameof(cartQueryHandler));
            _cartCommandHandler = cartCommandHandler ?? throw new ArgumentNullException(nameof(cartCommandHandler));
        }

        [HttpGet]
        public IActionResult AddToCart(string returnUrl)
        {
            var lines = HttpContext.Session.GetJson<List<CartLine>>("cart");
            var query = new CartQuery {Lines = lines, ReturnUrl = returnUrl};
            var result = _cartQueryHandler.Handle(query);

            return View("Cart", result);
        }

        [HttpPost]
        public RedirectResult AddToCart(long id, string returnUrl)
        {
            var lines = HttpContext.Session.GetJson<List<CartLine>>("cart") ?? new List<CartLine>();
            var command = new AddToCartCommand {BookId = id, Lines = lines};
            var newLines = _cartCommandHandler.Handle(command);

            HttpContext.Session.SetJson("cart", newLines);

            return Redirect(returnUrl);
            //return RedirectToAction("ShowBooks", "Store", new { returnUrl });
        }

        [HttpPost]
        public RedirectToActionResult RemoveFromCart(long bookId, string returnUrl)
        {
            var lines = HttpContext.Session.GetJson<List<CartLine>>("cart") ?? new List<CartLine>();
            var command = new DeleteFromCartCommand() {BookId = bookId, Lines = lines};
            var newLines = _cartCommandHandler.Handle(command);

            HttpContext.Session.SetJson("cart", newLines);
            return RedirectToAction("AddToCart", "Cart", new { returnUrl });
        }
    }
}
