using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Models.Categories;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Models.Properties;
using BooksStore.Domain.Contracts.Models.Publishers;
using BooksStore.Domain.Contracts.Services;

namespace BooksStore.Domain.Services
{
    public class PropertiesService : IPropertiesService
    {
        public List<FilterProperty> GetPublisherFilterProps()
        {
            return new List<FilterProperty>
            {
                new FilterProperty(nameof(PublisherResponse.Name), "Издательство", true),
                new FilterProperty(nameof(PublisherResponse.Country), "Страна нахождения", false),
                new FilterProperty(nameof(PublisherResponse.Created), "Дата создания издательства", false)
            };
        }

        public List<FilterProperty> GetCategoryFilterProps()
        {
            return new List<FilterProperty>
            {
                new FilterProperty(nameof(CategoryResponse.Name), "Категория", true),
                new FilterProperty(nameof(CategoryResponse.Created), "Дата создания издательства", false)
            };
        }

        public List<FilterSortingProps> GetBookFilterProps()
        {
            return new List<FilterSortingProps>
            {
                new FilterSortingProps(nameof(BookResponse.Title), "Название"),
                new FilterSortingProps(nameof(BookResponse.CategoryName), "Категория"),
                new FilterSortingProps(nameof(BookResponse.SubcategoryName), "Подкатегория"),
                new FilterSortingProps(nameof(BookResponse.PublisherName), "Издательство"),
                new FilterSortingProps(nameof(BookResponse.RetailPrice), "Цена")
            };
        }

        public List<SortingProperty> GetPublisherSortingProps(QueryConditions queryConditions)
        {
            var props = new List<SortingProperty>
            {
                new SortingProperty(nameof(Publisher.Id), "ID"),
                new SortingProperty(nameof(Publisher.Name), "Издательство"),
                new SortingProperty(nameof(Publisher.Country), "Страна нахождения издательства"),
                new SortingProperty(nameof(Publisher.Created), "Дата создания издательства")
            };

            foreach (var property in props)
            {
                var order = nameof(SortingOrder.Asc);

                if (property.PropertyName == queryConditions.OrderConditions[0].PropertyName)
                {
                    property.DescendingOrder = queryConditions.OrderConditions[0].PropertyValue == "DESC";
                    order = property.DescendingOrder == true ? nameof(SortingOrder.Asc) : nameof(SortingOrder.Desc);
                }

                property.UrlLink = $"/Publishers/Page{queryConditions.CurrentPage}/{property.PropertyName}/{order.ToUpper()}";
            }

            return props;
        }

        public List<SortingProperty> GetCategorySortingProps(QueryConditions queryConditions)
        {
            var props = new List<SortingProperty>
            {
                new SortingProperty(nameof(Category.Id), "ID"),
                new SortingProperty(nameof(Category.Name), "Категория"),
                new SortingProperty(nameof(CategoryResponse.Created), "Дата создания категории")
            };

            foreach (var property in props)
            {
                var order = nameof(SortingOrder.Asc);

                if (property.PropertyName == queryConditions.OrderConditions[0].PropertyName)
                {
                    property.DescendingOrder = queryConditions.OrderConditions[0].PropertyValue == "DESC";
                    order = property.DescendingOrder == true ? nameof(SortingOrder.Asc) : nameof(SortingOrder.Desc);
                }

                property.UrlLink = $"/Categories/Page{queryConditions.CurrentPage}/{property.PropertyName}/{order.ToUpper()}";
            }

            return props;
        }

        public List<SortingProperty> GetBooksSortingProps()
        {
            return new List<SortingProperty>
            {
                new SortingProperty(nameof(BookResponse.Id), "ID"),
                new SortingProperty(nameof(BookResponse.Title), "Название"),
                new SortingProperty(nameof(BookResponse.CategoryName), "Категория"),
                new SortingProperty(nameof(BookResponse.SubcategoryName), "Подкатегория"),
                new SortingProperty(nameof(BookResponse.PublisherName), "Издательство"),
                new SortingProperty(nameof(BookResponse.RetailPrice), "Цена")
            };
        }

        public List<ListItem> GetSortingProperties()
        {
            return new List<ListItem>
            {
                new ListItem("", "Сортировать по", false),
                new ListItem(nameof(BookResponse.Title), "Названию: А - Я"),
                new ListItem(nameof(BookResponse.Title), "Названию: Я - А", true),
                new ListItem(nameof(BookResponse.RetailPrice), "Цене: мин. - макс."),
                new ListItem(nameof(BookResponse.RetailPrice), "Цене: макс. - мин.", true)
            };
        }

        public List<ListItem> GetGridSizeProperties()
        {
            return new List<ListItem>
            {
                new ListItem("", "Отобразить", false),
                new ListItem("sixByTwo", "6 x 2 (строка x столбец)"),
                new ListItem("fourByThree", "4 x 3 (строка x столбец)"),
                new ListItem("threeByFour", "3 x 4 (строка x столбец)")
            };
        }
    }
}