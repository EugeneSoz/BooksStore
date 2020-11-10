using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BooksStore.App.Contracts.Query;
using BooksStore.Domain.Contracts.Models;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Models.Orders;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Models.Properties;
using BooksStore.Domain.Contracts.Repositories;
using BooksStore.Domain.Contracts.Services;
using BooksStore.Domain.Contracts.ViewModels;
using OnlineBooksStore.App.Handlers.Interfaces;

namespace BooksStore.App.Handlers.Query
{
    public class StoreBookQueryHandler : IQueryHandler<PageConditionsQuery, StoreBooksViewModel>
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IPagedListService<BookResponse> _pagedListService;
        private readonly IPropertiesService _propertiesService;
        private readonly IBookSqlQueryProcessingService _sqlQueryProcessingService;
        private readonly IMapper _mapper;

        public StoreBookQueryHandler(
            IBooksRepository booksRepository, 
            IPagedListService<BookResponse> pagedListService, 
            IPropertiesService propertiesService,
            IBookSqlQueryProcessingService sqlQueryProcessingService,
            IMapper mapper)
        {
            _booksRepository = booksRepository ?? throw new ArgumentNullException(nameof(booksRepository));
            _pagedListService = pagedListService ?? throw new ArgumentNullException(nameof(pagedListService));
            _propertiesService = propertiesService ?? throw new ArgumentNullException(nameof(propertiesService));
            _sqlQueryProcessingService = sqlQueryProcessingService ?? throw new ArgumentNullException(nameof(sqlQueryProcessingService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public StoreBooksViewModel Handle(PageConditionsQuery query)
        {
            var conditions = _mapper.Map<QueryConditions>(query);

            var sqlQueryConditions = _sqlQueryProcessingService.GenerateSqlQueryConditions(conditions);
            var (count, bookEntities) = _booksRepository.GetBooks(sqlQueryConditions);

            var categories = bookEntities
                .Select(be => _mapper.Map<BookResponse>(be));

            var response = _pagedListService.CreatePagedList(categories, count, conditions);

            var displayedBooksCount = response?.Entities?.Count ?? 0;
            var booksGrid = CreateBooksGrid(response?.Entities ?? new List<BookResponse>(), 4, displayedBooksCount);

            var result = new StoreBooksViewModel
            {
                Books = booksGrid,
                Category = conditions?.FilterConditions?[0].PropertyValue ?? "0",
                Pagination = response?.Pagination,
                ToolbarViewModel = new ToolbarViewModel
                {
                    SortingProperties = _propertiesService.GetSortingProperties(conditions),
                    GridSizeProperties = _propertiesService.GetGridSizeProperties(),
                    SortingProperty = new SortingProperty(conditions.OrderConditions[0].PropertyName, 
                        conditions.OrderConditions[0].PropertyValue),
                    DescendingOrder = conditions.OrderConditions[0].PropertyValue == nameof(SortingOrder.Desc).ToUpper()
                },
                AdminFilter = new AdminFilter
                {
                    SelectedProperty = conditions.FilterConditions != null ? conditions.FilterConditions[0].PropertyName : string.Empty,
                    SearchValue = string.Empty,
                    Controller = "Store",
                    Action = "ShowBooks"
                },
            };

            return result;
        }

        public BookResponse Handle(BookIdQuery query)
        {
            var bookEntity = _booksRepository.GetBook(query.Id);
            var book = _mapper.Map<BookResponse>(bookEntity);

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