using System;
using System.Collections.Generic;
using System.Linq;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Mapping;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Repositories;
using BooksStore.Domain.Contracts.Services;
using BooksStore.Domain.Contracts.ViewModels;
using OnlineBooksStore.App.Handlers.Interfaces;

namespace BooksStore.App.Handlers.Query
{
    public class BookQueryHandler : 
        IQueryHandler<StorePageFilterQuery, BooksViewModel>,
        IQueryHandler<PageFilterQuery, PagedList<BookResponse>>,
        IQueryHandler<BookIdQuery, BookResponse>
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IPagedListService<BookResponse> _pagedListService;

        public BookQueryHandler(IBooksRepository booksRepository, IPagedListService<BookResponse> pagedListService)
        {
            _booksRepository = booksRepository ?? throw new ArgumentNullException(nameof(booksRepository));
            _pagedListService = pagedListService ?? throw new ArgumentNullException(nameof(pagedListService));
        }

        public BooksViewModel Handle(StorePageFilterQuery query)
        {
            PageFilterQuery pageFilter = query;
            var response = Handle(pageFilter);
            var displayedBooksCount = response?.Entities?.Count ?? 0;
            var booksGrid = CreateBooksGrid(response?.Entities ?? new List<BookResponse>(), 4, displayedBooksCount);

            var result = new BooksViewModel
            {
                Books = booksGrid,
                Pagination = response?.Pagination
            };

            return result;
        }

        public PagedList<BookResponse> Handle(PageFilterQuery query)
        {
            var options = query.MapQueryOptions();
            var bookEntities = _booksRepository.GetBooks(options);
            var books = bookEntities.books
                .Select(b => b.MapBookResponse())
                .ToList();

            var result = _pagedListService.CreatePagedList(books, bookEntities.count, options);

            return result;
        }

        public BookResponse Handle(BookIdQuery query)
        {
            var bookEntity = _booksRepository.GetBook(query.Id);
            var book = bookEntity.MapBookResponse();

            return book;
        }

        private Dictionary<int, List<BookResponse>> CreateBooksGrid(List<BookResponse> books, int cardsInRow, int displayedBooksCount)
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