using System;
using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Services;

namespace BooksStore.Domain.Services
{
    public class PagedListService<T> : IPagedListService<T>
    {
        /// <summary>
        /// The displayed pages count in pagination component
        /// </summary>
        private readonly int _displayedPagesCount = 5;
        /// <summary>
        /// The pages count to the left from current page
        /// </summary>
        private int _leftPagesCount;
        /// <summary>
        /// The pages count to the right from current page
        /// </summary>
        private int _rightPagesCount;
        /// <summary>
        /// The pages count appended to left boundary in case of choosing the extreme value of the right border
        /// </summary>
        private int _leftDelta;
        /// <summary>
        /// The pages count appended to right boundary in case of choosing the extreme value of the left border
        /// </summary>
        private int _rightDelta;

        public PagedList<T> CreatePagedList(List<T> entities, int pagesCount, PageOptions options)
        {
            if (options == null)
            {
                return CreateDefaultPagedList();
            }
            _leftPagesCount = _displayedPagesCount / 2;
            _rightPagesCount = _displayedPagesCount / 2;
            var totalPages = (int)Math.Ceiling(pagesCount / (double)options.PageSize);
            //то, что нужно добавить к левой границе
            _leftDelta = options.CurrentPage + _rightPagesCount > totalPages
                ? options.CurrentPage + _rightPagesCount - totalPages
                : 0;
            var leftBoundary = options.CurrentPage - _leftPagesCount - _leftDelta;

            _rightDelta = options.CurrentPage - _leftPagesCount < 1
                ? 1 - (options.CurrentPage - _leftPagesCount)
                : 0;
            var rightBoundary = options.CurrentPage + _rightPagesCount + _rightDelta;

            var pagedList = new PagedList<T>
            {
                Entities = new List<T>(entities),
                Pagination = new Pagination()
                {
                    CurrentPage = options.CurrentPage,
                    PageSize = options.PageSize,
                    TotalPages = totalPages,
                    LeftBoundary = leftBoundary <= 0 ? 1 : leftBoundary,
                    RightBoundary = rightBoundary > totalPages ? totalPages : rightBoundary,
                }
            };

            pagedList.Pagination.PageNumbers =
                CreatePageNumbers(pagedList.Pagination.LeftBoundary, pagedList.Pagination.RightBoundary);

            return pagedList;
        }

        private PagedList<T> CreateDefaultPagedList()
        {
            return new PagedList<T>
            {
                Entities = new List<T>(),
                Pagination = new Pagination()
                {
                    CurrentPage = 1,
                    PageSize = 1,
                    TotalPages = 1,
                    LeftBoundary = 1,
                    RightBoundary = 1,
                    PageNumbers = new List<int> {1}
                }
            };
        }

        private List<int> CreatePageNumbers(int leftBoundary, int rightBoundary)
        {
            var numbers = new List<int>();
            for (int i = leftBoundary; i <= rightBoundary; i++)
            {
                numbers.Add(i);
            }

            return numbers;
        }
    }
}
