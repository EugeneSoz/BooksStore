using System.Collections.Generic;

namespace BooksStore.Domain.Contracts.Models.Pages
{
    public class Pagination
    {
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
        public int LeftBoundary { get; set; }
        /// <summary>
        /// Gets the right boundary page value.
        /// </summary>
        /// <value>
        /// The right boundary page value.
        /// </value>
        public int RightBoundary { get; set; }
        /// <summary>
        /// Gets or sets the page numbers.
        /// </summary>
        /// <value>
        /// The page numbers.
        /// </value>
        public List<int> Pages { get; set; }
    }
}
