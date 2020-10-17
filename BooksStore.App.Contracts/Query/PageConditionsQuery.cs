using BooksStore.Domain.Contracts.Models;

namespace BooksStore.App.Contracts.Query
{
    public class PageConditionsQuery : Query
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string PropertyName { get; set; }
        public string Order { get; set; }
        public string FilterPropertyName { get; set; }
        public string FilterPropertyValue { get; set; }
        public FilterAction FilterAction { get; set; }
    }
}