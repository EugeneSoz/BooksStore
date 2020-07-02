using BooksStore.App.Contracts.Query;
using BooksStore.Domain.Contracts.Models.Pages;

namespace BooksStore.App.Handlers.Mapping
{
    public static class PageOptionsMappings
    {
        public static QueryOptions MapQueryOptions(this PageFilterQuery query)
        {
            return new QueryOptions
            {
                CurrentPage = query.CurrentPage,
                PageSize = query.PageSize,
                DescendingOrder = query.DescendingOrder,
                FilterPropertyName = query.FilterPropertyName,
                FilterPropertyValue = query.FilterPropertyValue,
                SearchPropertyNames = query.SearchPropertyNames,
                SearchTerm = query.SearchTerm,
                SortPropertyName = query.SortPropertyName
            };
        }
    }
}