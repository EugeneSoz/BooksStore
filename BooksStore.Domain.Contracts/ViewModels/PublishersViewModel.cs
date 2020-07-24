using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Models.Publishers;

namespace BooksStore.Domain.Contracts.ViewModels
{
    public class PublishersViewModel
    {
        public List<PublisherResponse> Publishers { get; set; }
        public Pagination Pagination { get; set; }
        public AdminToolbarViewModel ToolbarViewModel { get; set; }
        public List<FilterSortingProps> FilterProps { get; set; }
        public List<FilterSortingProps> TableHeaders { get; set; }
    }
}