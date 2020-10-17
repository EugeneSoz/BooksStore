using BooksStore.App.Contracts.Query;
using BooksStore.Domain.Contracts.Models.Pages;

namespace BooksStore.App.Handlers.Mapping
{
    public static class PageOptionsMappings
    {
        public static PageOptions1 MapToPageOptions(this PageFilterQuery query)
        {
            return new PageOptions1
            {
                CurrentPage = query.CurrentPage,
                PageSize = query.PageSize,
                DescendingOrder = query.DescendingOrder,
                FilterPropertyName = query.FilterPropertyName,
                FilterPropertyValue = query.FilterPropertyValue,
                SearchPropertyNames = query.SearchPropertyNames,
                SearchTerm = query.SearchTerm,
                SortPropertyName = query.SortPropertyName,
            };
        }
    }
}