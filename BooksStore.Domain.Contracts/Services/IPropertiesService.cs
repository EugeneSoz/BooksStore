using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models;
using OnlineBooksStore.Domain.Contracts.Models;

namespace BooksStore.Domain.Contracts.Services
{
    public interface IPropertiesService
    {
        List<FilterSortingProps> GetPublisherFilterProps();
        List<FilterSortingProps> GetCategoryFilterProps();
        List<FilterSortingProps> GetBookFilterProps();
        List<FilterSortingProps> GetPublisherSortingProps();
        List<FilterSortingProps> GetCategorySortingProps();
        List<FilterSortingProps> GetBooksSortingProps();
        List<ListItem> GetSortingProperties();
        List<ListItem> GetGridSizeProperties();
    }
}