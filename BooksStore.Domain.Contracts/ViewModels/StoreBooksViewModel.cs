using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Models.Pages;

namespace BooksStore.Domain.Contracts.ViewModels
{
    public class StoreBooksViewModel
    {
        public Dictionary<int, List<BookResponse>> Books { get; set; }
        public Pagination Pagination { get; set; }
        public ToolbarViewModel ToolbarViewModel { get; set; }
    }
}