using System.Collections.Generic;

namespace BooksStore.Persistence.Entities
{
    /// <summary>
    /// Represents a book category
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public class CategoryEntity : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the related books.
        /// </summary>
        /// <value>
        /// The related books.
        /// </value>
        public List<BookEntity> Books { get; set; }
    }
}
