using System;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Query;
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
        public IActionResult ShowBooks(int page)
        {
            var query1 = new StorePageFilterQuery()
            {
                CurrentPage = page == 0 ? 1 : page,
                PageSize = 20,
            };

            var result = _bookQueryHandler.Handle(query1);
            
            return View("Store", result);
        }
    }
}
