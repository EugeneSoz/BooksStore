using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models;
using BooksStore.Domain.Contracts.Models.Properties;

namespace BooksStore.Domain.Contracts.ViewModels
{
    public class ToolbarViewModel
    {
        public List<ListItem> GridSizeProperties { get; set; }
        public List<SortingProperty> SortingProperties { get; set; }
        public SortingProperty SortingProperty { get; set; }
        public bool? DescendingOrder { get; set; }
    }
}