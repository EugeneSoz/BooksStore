using System;
using System.Collections.Generic;
using System.Linq;

namespace BooksStore.Domain.Contracts.Models.Pages
{
    public class PagedList<T>
    {
        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        /// <value>
        /// The entities.
        /// </value>
        public List<T> Entities { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="Pagination"/> object.
        /// </summary>
        /// <value>
        /// The <see cref="Pagination"/> object.
        /// </value>
        public Pagination Pagination { get; set; }
    }
}
