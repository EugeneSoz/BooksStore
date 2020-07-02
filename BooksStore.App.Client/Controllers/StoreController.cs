using System;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Query;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.App.Client.Controllers
{
    public class StoreController : Controller
    {
        private readonly BookQueryHandler _bookQueryHandler;

        public StoreController(BookQueryHandler bookQueryHandler)
        {
            _bookQueryHandler = bookQueryHandler ?? throw new ArgumentNullException(nameof(bookQueryHandler));
        }

        public IActionResult ShowBooks()
        {
            var query = new StorePageFilterQuery()
            {
                CurrentPage = 1,
                PageSize = 20
            };

            var result = _bookQueryHandler.Handle(query);
            
            return View("Store", result);
        }
    }
}
