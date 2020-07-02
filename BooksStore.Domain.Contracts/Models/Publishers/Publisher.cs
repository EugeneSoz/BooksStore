using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Books;
using OnlineBooksStore.Domain.Contracts.Models.Publishers;

namespace BooksStore.Domain.Contracts.Models.Publishers
{
    /// <summary>
    /// Represents a book publisher
    /// </summary>
    public class Publisher : PublisherResponse
    {
        /// <summary>
        /// Gets or sets the related books.
        /// </summary>
        /// <value>
        /// The related books.
        /// </value>
        public List<RelatedBook> Books { get; set; }
    }
}
