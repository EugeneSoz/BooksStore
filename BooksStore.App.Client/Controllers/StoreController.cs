using System;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Query;
using BooksStore.Domain.Contracts.Models.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;

namespace BooksStore.App.Client.Controllers
{
    public class StoreController : Controller
    {
        private readonly BookQueryHandler _bookQueryHandler;

        public StoreController(BookQueryHandler bookQueryHandler)
        {
            _bookQueryHandler = bookQueryHandler ?? throw new ArgumentNullException(nameof(bookQueryHandler));
        }

        [HttpGet]
        public IActionResult ShowBooks(int page, string category)
        {
            var query = new StorePageFilterQuery()
            {
                CurrentPage = page == 0 ? 1 : page,
                PageSize = 20,
                FilterPropertyName = string.IsNullOrEmpty(category) ? null : nameof(BookResponse.CategoryId),
                FilterPropertyValue = string.IsNullOrEmpty(category) ? 0 : long.Parse(category)
            };

            var result = _bookQueryHandler.Handle(query);
            
            return View("Store", result);
        }

        [HttpGet]
        public IActionResult ShowDetails(long bookId)
        {
            var query = new BookIdQuery { Id = bookId };
            var result = _bookQueryHandler.Handle(query);

            return View("StoreBookDetails", result);
        }
    }
}
