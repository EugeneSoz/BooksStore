using System;
using System.Collections.Generic;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Query;
using BooksStore.Domain.Contracts.Models;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Models.Properties;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.App.Client.Controllers
{
    public class StoreController : Controller
    {
        private readonly StoreBookQueryHandler _bookQueryHandler;

        public StoreController(StoreBookQueryHandler bookQueryHandler)
        {
            _bookQueryHandler = bookQueryHandler ?? throw new ArgumentNullException(nameof(bookQueryHandler));
        }

        [HttpGet]
        [HttpPost]
        public IActionResult ShowBooks(AdminFilter adminFilter, PageOptions pageOptions)
        {
            var query = new PageConditionsQuery(adminFilter, pageOptions);

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
