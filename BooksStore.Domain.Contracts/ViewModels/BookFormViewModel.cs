using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Books;

namespace BooksStore.Domain.Contracts.ViewModels
{
    public class BookFormViewModel
    {
        public Book Book { get; set; }
        public BookResponse BookResponse { get; set; }
        public AdminToolbarViewModel ToolbarViewModel { get; set; }
    }
}