using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Models.Categories;

namespace BooksStore.Domain.Contracts.ViewModels
{
    public class CategoryFormViewModel
    {
        public Category Category { get; set; }
        public AdminToolbarViewModel ToolbarViewModel { get; set; }
        public List<RelatedBook> RelatedBooks { get; set; }
    }
}