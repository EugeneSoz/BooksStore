using BooksStore.Domain.Contracts.Models;
using BooksStore.Domain.Contracts.Models.Pages;

namespace BooksStore.App.Contracts.Query
{
    public class PageConditionsQuery : Query
    {
        public PageConditionsQuery() { }

        public PageConditionsQuery(AdminFilter adminFilter, PageOptions pageOptions)
        {
            CurrentPage = pageOptions.Page;
            PageSize = 20;
            PropertyName = pageOptions.PropertyName;
            Order = pageOptions.Order;
            SelectedPropertyName = adminFilter?.SelectedProperty;
            SearchValue = adminFilter?.SearchValue;
            FirstRangeValue = adminFilter?.FirstRangeValue;
            SecondRangeValue = adminFilter?.SecondRangeValue;
            FormAction = adminFilter?.FormAction ?? FormAction.Cancel;
        }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string PropertyName { get; set; }
        public string Order { get; set; }
        public string SelectedPropertyName { get; set; }
        public string SearchValue { get; set; }
        public string FirstRangeValue { get; set; }
        public string SecondRangeValue { get; set; }
        public FormAction FormAction { get; set; }
    }
}