using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Properties;

namespace BooksStore.Domain.Contracts.Models
{
    public class AdminFilter
    {
        public List<FilterProperty> FilterProperties { get; set; }
        public string SelectedProperty { get; set; }
        public string SearchValue { get; set; }
        public FilterAction FilterAction { get; set; }
    }
}