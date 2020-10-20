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
        IQueryHandler<StorePageFilterQuery, StoreBooksViewModel>,
        IQueryHandler<PageFilterQuery, BooksViewModel>,
        IQueryHandler<BookIdQuery, BookResponse>
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IPagedListService<BookResponse> _pagedListService;
        private readonly IPropertiesService _propertiesService;
        private readonly ISqlQueryProcessingService _sqlQueryProcessingService;

        public BookQueryHandler(
            IBooksRepository booksRepository, 
            IPagedListService<BookResponse> pagedListService, 
            IPropertiesService propertiesService,
            ISqlQueryProcessingService sqlQueryProcessingService)
        {
            _booksRepository = booksRepository ?? throw new ArgumentNullException(nameof(booksRepository));
            _pagedListService = pagedListService ?? throw new ArgumentNullException(nameof(pagedListService));
            _propertiesService = propertiesService ?? throw new ArgumentNullException(nameof(propertiesService));
            _sqlQueryProcessingService = sqlQueryProcessingService ?? throw new ArgumentNullException(nameof(sqlQueryProcessingService));
        }

        public StoreBooksViewModel Handle(StorePageFilterQuery query)
        {
            PageFilterQuery pageFilter = query;
            var conditions = new QueryConditions
            {
                CurrentPage = query.CurrentPage,
                PageSize = query.PageSize
            };
            var response = Handle1(conditions);
            var displayedBooksCount = response?.Entities?.Count ?? 0;
            var booksGrid = CreateBooksGrid(response?.Entities ?? new List<BookResponse>(), 4, displayedBooksCount);

            var result = new StoreBooksViewModel
            {
                Books = booksGrid,
                Pagination = response?.Pagination,
                ToolbarViewModel = new ToolbarViewModel
                {
                    SortingProperties = _propertiesService.GetGridSizeProperties(),
                    GridSizeProperties = _propertiesService.GetSortingProperties()
                }
            };

            return result;
        }

        private PagedList<BookResponse> Handle1(QueryConditions query)
        {
            var conditions = new QueryConditions
            {
                CurrentPage = query.CurrentPage,
                PageSize = query.PageSize
            };

            var sqlQueryConditions = _sqlQueryProcessingService.GenerateSqlQueryConditions(conditions);
            var bookEntities = _booksRepository.GetBooks(sqlQueryConditions);
            var books = bookEntities.books
                .Select(b => b.MapBookResponse())
                .ToList();

            var result = _pagedListService.CreatePagedList(books, bookEntities.count, conditions);

            return result;
        }

        public BooksViewModel Handle(PageFilterQuery query)
        {
            var conditions = new QueryConditions
            {
                CurrentPage = query.CurrentPage,
                PageSize = query.PageSize
            };
            var sqlQueryConditions = _sqlQueryProcessingService.GenerateSqlQueryConditions(conditions);
            var options = query.MapToPageOptions();
            var bookEntities = _booksRepository.GetBooks(sqlQueryConditions);
            var books = bookEntities.books
                .Select(b => b.MapBookResponse())
                .ToList();

            var result = _pagedListService.CreatePagedList(books, bookEntities.count, conditions);

            var viewModel = new BooksViewModel()
            {
                Books = result.Entities,
                Pagination = result.Pagination,
                ToolbarViewModel = new AdminToolbarViewModel
                {
                    FormUrl = string.Empty,
                    IsFormButtonVisible = true
                },
                FilterProps = _propertiesService.GetBookFilterProps(),
                TableHeaders = _propertiesService.GetBooksSortingProps()
            };
            return viewModel;
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