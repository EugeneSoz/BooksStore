using System;
using System.Collections.Generic;
using System.Linq;

namespace BooksStore.Domain.Contracts.Models.Pages
{
    public class PagedList<T>
    {
        /// <summary>
        /// The displayed pages count in pagination component
        /// </summary>
        private readonly int _displayedPagesCount = 5;
        /// <summary>
        /// The pages count to the left from current page
        /// </summary>
        private readonly int _leftPagesCount;
        /// <summary>
        /// The pages count to the right from current page
        /// </summary>
        private readonly int _rightPagesCount;
        /// <summary>
        /// The pages count appended to left boundary in case of choosing the extreme value of the right border
        /// </summary>
        private int _leftDelta;
        /// <summary>
        /// The pages count appended to right boundary in case of choosing the extreme value of the left border
        /// </summary>
        private int _rightDelta;

        public PagedList(IQueryable<T> query, QueryOptions options = null)
        {
            _leftPagesCount = _rightPagesCount = _displayedPagesCount / 2;
            CurrentPage = options?.CurrentPage ?? 1;
            PageSize = options.PageSize;
            TotalPages = (int)Math.Ceiling(query.Count() / (double)PageSize);
            Entities = new List<T>();
            Entities.AddRange(query.Skip((CurrentPage - 1) * PageSize).Take(PageSize));
        }

        public PagedList(IEnumerable<T> entities, int pagesCount, QueryOptions options = null)
        {
            _leftPagesCount = _rightPagesCount = _displayedPagesCount / 2;
            CurrentPage = options?.CurrentPage ?? 1;
            PageSize = options?.PageSize ?? 1;
            TotalPages = (int)Math.Ceiling(pagesCount / (double)PageSize);
            Entities = new List<T>();
            Entities.AddRange(entities);
        }

        public PagedList()
        {
            
        }
        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        /// <value>
        /// The entities.
        /// </value>
        public List<T> Entities { get; set; }
        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        public int CurrentPage { get; set; }
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int PageSize { get; set; }
        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        /// <value>
        /// The total pages.
        /// </value>
        public int TotalPages { get; set; }
        /// <summary>
        /// Gets a value indicating whether this instance has previous page.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has previous page; otherwise, <c>false</c>.
        /// </value>
        public bool HasPreviousPage => CurrentPage > 1;
        /// <summary>
        /// Gets a value indicating whether this instance has next page.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has next page; otherwise, <c>false</c>.
        /// </value>
        public bool HasNextPage => CurrentPage < TotalPages;
        /// <summary>
        /// Gets the left boundary page value.
        /// </summary>
        /// <value>
        /// The left boundary page value.
        /// </value>
        public int LeftBoundary
        {
            get
            {
                //то, что нужно добавить к левой границе
                _leftDelta = CurrentPage + _rightPagesCount > TotalPages
                    ? CurrentPage + _rightPagesCount - TotalPages
                    : 0;
                int leftBoundary = CurrentPage - _leftPagesCount - _leftDelta;
                return leftBoundary <= 0 ? 1 : leftBoundary;
            }
        }
        /// <summary>
        /// Gets the right boundary page value.
        /// </summary>
        /// <value>
        /// The right boundary page value.
        /// </value>
        public int RightBoundary
        {
            get
            {
                _rightDelta = CurrentPage - _leftPagesCount < 1
                    ? 1 - (CurrentPage - _leftPagesCount)
                    : 0;
                int rightBoundary = CurrentPage + _rightPagesCount + _rightDelta;
                return rightBoundary > TotalPages ? TotalPages : rightBoundary;
            }
        }
    }
}
