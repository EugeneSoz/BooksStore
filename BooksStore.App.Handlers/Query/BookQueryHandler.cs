﻿using System;
using System.Linq;
using AutoMapper;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Mapping;
using BooksStore.Domain.Contracts.Models;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Models.Properties;
using BooksStore.Domain.Contracts.Repositories;
using BooksStore.Domain.Contracts.Services;
using BooksStore.Domain.Contracts.ViewModels;
using OnlineBooksStore.App.Handlers.Interfaces;

namespace BooksStore.App.Handlers.Query
{
    public class BookQueryHandler : 
        IQueryHandler<PageConditionsQuery, BooksViewModel>,
        IQueryHandler<BookIdQuery, BookResponse>
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IPagedListService<BookResponse> _pagedListService;
        private readonly IPropertiesService _propertiesService;
        private readonly IBookSqlQueryProcessingService _sqlQueryProcessingService;
        private readonly IMapper _mapper;

        public BookQueryHandler(
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

        public BooksViewModel Handle(PageConditionsQuery query)
        {
            var conditions = _mapper.Map<QueryConditions>(query);

            var sqlQueryConditions = _sqlQueryProcessingService.GenerateSqlQueryConditions(conditions);
            var (count, bookEntities) = _booksRepository.GetBooks(sqlQueryConditions);

            var categories = bookEntities
                .Select(be => _mapper.Map<BookResponse>(be));

            var result = _pagedListService.CreatePagedList(categories, count, conditions);

            var viewModel = new BooksViewModel
            {
                Entities = result.Entities,
                Pagination = result.Pagination,
                ToolbarViewModel = new AdminToolbarViewModel
                {
                    FormUrl = string.Empty,
                    IsFormButtonVisible = true
                },
                AdminFilter = new AdminFilter
                {
                    FilterProperties = _propertiesService.GetBookFilterProps(),
                    SelectedProperty = conditions.FilterConditions != null ? conditions.FilterConditions[0].PropertyName : string.Empty,
                    SearchValue = string.Empty,
                    Controller = "Books",
                    Action = "ShowBooks"
                },
                TableHeaders = _propertiesService.GetBooksSortingProps(conditions),
                SortingProperty = new SortingProperty(conditions.OrderConditions[0].PropertyName, 
                    conditions.OrderConditions[0].PropertyValue)
            };
            return viewModel;
        }

        public BookResponse Handle(BookIdQuery query)
        {
            var bookEntity = _booksRepository.GetBook(query.Id);
            var book = _mapper.Map<BookResponse>(bookEntity);

            return book;
        }
    }
}