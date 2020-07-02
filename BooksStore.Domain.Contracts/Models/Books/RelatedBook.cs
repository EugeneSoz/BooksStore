using OnlineBooksStore.Domain.Contracts.Models;

namespace BooksStore.Domain.Contracts.Models.Books
{
    /// <summary>
    /// Represents a related book
    /// </summary>
    public class RelatedBook : EntityBase
    {
        /// <summary>
        /// Gets or sets the book title.
        /// </summary>
        /// <value>
        /// The book title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the authors.
        /// </summary>
        /// <value>
        /// The authors.
        /// </value>
        public string Authors { get; set; }
        /// <summary>
        /// Gets or sets the purchase book price.
        /// </summary>
        /// <value>
        /// The book purchase price.
        /// </value>
        public decimal PurchasePrice { get; set; }

        /// <summary>
        /// Gets or sets the retail book price.
        /// </summary>
        /// <value>
        /// The retail book price.
        /// </value>
        public decimal RetailPrice { get; set; }
    }
}