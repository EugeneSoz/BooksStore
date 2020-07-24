using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Models.Publishers;

namespace BooksStore.Domain.Contracts.ViewModels
{
    public class PublisherFormViewModel
    {
        public Publisher Publisher { get; set; }
        public AdminToolbarViewModel ToolbarViewModel { get; set; }
        public List<RelatedBook> RelatedBooks { get; set; }
    }
}