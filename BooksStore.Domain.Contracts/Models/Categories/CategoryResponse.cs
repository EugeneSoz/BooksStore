using System;

namespace BooksStore.Domain.Contracts.Models.Categories
{
    public class CategoryResponse : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        //если свойство не равно null, тогда категория является подкатегорией
        public long? ParentId { get; set; }
        public string ParentCategoryName { get; set; }
        public string DisplayedName { get; set; }
        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        public DateTime Created { get; set; }
    }
}
