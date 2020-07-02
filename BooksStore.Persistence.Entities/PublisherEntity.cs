using System.Collections.Generic;

namespace BooksStore.Persistence.Entities
{
    /// <summary>
    /// Represents a book publisher
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public class PublisherEntity : BaseEntity
    {
        /// <summary>
        /// Gets or sets the publisher name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the publisher country of origin.
        /// </summary>
        /// <value>
        /// The country of origin.
        /// </value>
        public string Country { get; set; }
        /// <summary>
        /// Gets or sets the related books.
        /// </summary>
        /// <value>
        /// The related books.
        /// </value>
        public List<BookEntity> Books { get; set; }
    }
}
