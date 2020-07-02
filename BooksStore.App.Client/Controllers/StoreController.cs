using System;
using System.Collections.Generic;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Query;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.ViewModels;
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
            var query = new PageFilterQuery()
            {
                CurrentPage = 1,
                PageSize = 20
            };

            var result = _bookQueryHandler.Handle(query);
            var displayedBooksCount = result?.Entities?.Count ?? 0;
            var model = CreateBookGrid(result.Entities, 4, displayedBooksCount);

            var booksInfo = new BooksInfo
            {
                Books = model,
                Pagination = result.Pagination
            };

            return View("Store", booksInfo);
        }

        private Dictionary<int, List<BookResponse>> CreateBookGrid(List<BookResponse> books, int cardsInRow, int displayedBooksCount)
        {
            var booksGrid = new Dictionary<int, List<BookResponse>>();
            var rowsCount = Math.Ceiling(displayedBooksCount / (double)cardsInRow);
            for (int row = 0; row < rowsCount; row++)
            {
                booksGrid.Add(row, new List<BookResponse>());
                for (int col = 0; col < cardsInRow; col++)
                {
                    if (IsColEmpty(row, col, displayedBooksCount))
                    {
                        booksGrid[row].Add(null);
                    }
                    else
                    {
                        var index = GetBookIndex(row, col);
                        booksGrid[row].Add(books[index]);
                    }
                }
            }

            return booksGrid;
        }

        private int GetBookIndex(int row, int column) {
            return 4 * row + column;
        }

        private bool IsColEmpty(int row, int column, int displayedBooksCount) {
            return GetBookIndex(row, column) >= displayedBooksCount;
        }
    }
}
