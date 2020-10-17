using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Properties;

namespace BooksStore.Domain.Contracts.Models
{
    public class AdminFilter
    {
        public List<FilterProperty> FilterProperties { get; set; }
        public string SelectedProperty { get; set; }
        public string SearchValue { get; set; }
        public string FirstRangeValue { get; set; }
        public string SecondRangeValue { get; set; }
        public FormAction FormAction { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}