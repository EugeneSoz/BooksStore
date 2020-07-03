using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models;

namespace BooksStore.Domain.Contracts.ViewModels
{
    public class ToolbarViewModel
    {
        public List<ListItem> GridSizeProperties { get; set; }
        public List<ListItem> SortingProperties { get; set; }
    }
}