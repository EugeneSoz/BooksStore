﻿using System.Collections.Generic;
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
        List<FilterProperty> GetBookFilterProps();
        List<SortingProperty> GetPublisherSortingProps(QueryConditions queryConditions);
        List<SortingProperty> GetCategorySortingProps(QueryConditions queryConditions);
        List<SortingProperty> GetBooksSortingProps(QueryConditions queryConditions);
        List<SortingProperty> GetSortingProperties(QueryConditions queryConditions);
        List<ListItem> GetGridSizeProperties();
    }
}