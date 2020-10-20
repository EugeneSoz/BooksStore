using System;

namespace BooksStore.Domain.Contracts.Models
{
    /// <summary>
    /// Base entity class with the identifier
    /// </summary>
    public abstract class EntityBase
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }
        public string DisplayedName { get; set; }
    }
}
