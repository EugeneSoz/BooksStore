using System;

namespace BooksStore.Persistence.Entities
{
    /// <summary>
    /// Base entity class with the identifier
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }
        /// <summary>
        /// Gets or sets the created date with time.
        /// </summary>
        /// <value>
        /// The created date with time.
        /// </value>
        public DateTime Created { get; set; }
        /// <summary>
        /// Gets or sets the updated date with time.
        /// </summary>
        /// <value>
        /// The updated date with time.
        /// </value>
        public DateTime Updated { get; set; }  
    }
}
