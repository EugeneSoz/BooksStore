using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Models.Properties;
using OnlineBooksStore.Domain.Contracts.Models;

namespace BooksStore.Domain.Contracts.Services
{
    public interface IPropertiesService
    {
        List<FilterProperty> GetPublisherFilterProps();
        List<FilterProperty> GetCategoryFilterProps();
        List<FilterSortingProps> GetBookFilterProps();
        List<SortingProperty> GetPublisherSortingProps(QueryConditions queryConditions);
        List<SortingProperty> GetCategorySortingProps(QueryConditions queryConditions);
        List<SortingProperty> GetBooksSortingProps();
        List<ListItem> GetSortingProperties();
        List<ListItem> GetGridSizeProperties();
    }
}