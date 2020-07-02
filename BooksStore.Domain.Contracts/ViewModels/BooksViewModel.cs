using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Models.Pages;

namespace BooksStore.Domain.Contracts.ViewModels
{
    public class BooksViewModel
    {
        public Dictionary<int, List<BookResponse>> Books { get; set; }
        public Pagination Pagination { get; set; }
    }
}