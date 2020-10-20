using System;

namespace BooksStore.Domain.Contracts.Models.Categories
{
    public class CategoryResponse : EntityBase
    {
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        public DateTime Created { get; set; }
    }
}
