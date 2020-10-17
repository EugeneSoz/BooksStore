using BooksStore.Domain.Contracts.Models;

namespace BooksStore.App.Contracts.Query
{
    public class PageConditionsQuery : Query
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string PropertyName { get; set; }
        public string Order { get; set; }
        public string SearchPropertyName { get; set; }
        public string SearchPropertyValue { get; set; }
        public FormAction FormAction { get; set; }
    }
}