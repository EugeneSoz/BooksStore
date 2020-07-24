﻿using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Models.Pages;

namespace BooksStore.Domain.Contracts.ViewModels
{
    public class BooksViewModel
    {
        public List<BookResponse> Books { get; set; }
        public Pagination Pagination { get; set; }
        public AdminToolbarViewModel ToolbarViewModel { get; set; }
        public List<FilterSortingProps> FilterProps { get; set; }
        public List<FilterSortingProps> TableHeaders { get; set; }
    }
}