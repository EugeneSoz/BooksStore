using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Models.Properties;

namespace BooksStore.Domain.Contracts.ViewModels
{
    public class StoreBooksViewModel
    {
        public Dictionary<int, List<BookResponse>> Books { get; set; }
        public Pagination Pagination { get; set; }
        public string Category { get; set; }
        public ToolbarViewModel ToolbarViewModel { get; set; }
        public AdminFilter AdminFilter { get; set; }
    }
}