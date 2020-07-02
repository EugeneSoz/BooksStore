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
        //если свойство не равно null, тогда категория является подкатегорией        
        /// <summary>
        /// Gets or sets the parent category identifier.
        /// </summary>
        /// <value>
        /// The parent category identifier.
        /// </value>
        public long? ParentId { get; set; }
        //для сортировки по имени родительской и текущей категории        
        /// <summary>
        /// Gets or sets the name in "Parent category name => category name" format.
        /// </summary>
        /// <value>
        /// The name of the displayed category.
        /// </value>
        public string ParentAndChildName { get; set; }
        /// <summary>
        /// Gets or sets the parent category.
        /// </summary>
        /// <value>
        /// The parent category.
        /// </value>
        public CategoryEntity ParentCategory { get; set; }
        /// <summary>
        /// Gets or sets the children categories.
        /// </summary>
        /// <value>
        /// The children categories.
        /// </value>
        public List<CategoryEntity> ChildrenCategories { get; set; }
        /// <summary>
        /// Gets or sets the related books.
        /// </summary>
        /// <value>
        /// The related books.
        /// </value>
        public List<BookEntity> Books { get; set; }
    }
}
