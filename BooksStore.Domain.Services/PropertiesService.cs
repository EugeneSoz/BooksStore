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

        public List<FilterProperty> GetBookFilterProps()
        {
            return new List<FilterProperty>
            {
                new FilterProperty(nameof(BookResponse.Title), "Название", true),
                new FilterProperty(nameof(BookResponse.CategoryName), "Категория", false),
                new FilterProperty(nameof(BookResponse.PublisherName), "Издательство", false),
                new FilterProperty(nameof(BookResponse.PurchasePrice), "Цена закупки", false),
                new FilterProperty(nameof(BookResponse.RetailPrice), "Цена продажи", false),
                new FilterProperty(nameof(BookResponse.Created), "Дата создания книги", false)
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

        public List<SortingProperty> GetBooksSortingProps(QueryConditions queryConditions)
        {
            var props = new List<SortingProperty>
            {
                new SortingProperty(nameof(BookResponse.Id), "ID"),
                new SortingProperty(nameof(BookResponse.Title), "Название"),
                new SortingProperty(nameof(BookResponse.CategoryName), "Категория"),
                new SortingProperty(nameof(BookResponse.PublisherName), "Издательство"),
                new SortingProperty(nameof(BookResponse.PurchasePrice), "Цена закупки"),
                new SortingProperty(nameof(BookResponse.RetailPrice), "Цена продажи"),
                new SortingProperty(nameof(BookResponse.Created), "Дата создания книги"),
            };

            foreach (var property in props)
            {
                var order = nameof(SortingOrder.Asc);

                if (property.PropertyName == queryConditions.OrderConditions[0].PropertyName)
                {
                    property.DescendingOrder = queryConditions.OrderConditions[0].PropertyValue == nameof(SortingOrder.Desc).ToUpper();
                    order = property.DescendingOrder == true ? nameof(SortingOrder.Asc) : nameof(SortingOrder.Desc);
                }

                property.UrlLink = $"/Books/Page{queryConditions.CurrentPage}/{property.PropertyName}/{order.ToUpper()}";
            }

            return props;
        }

        public List<SortingProperty> GetSortingProperties(QueryConditions queryConditions)
        {
            var props = new List<SortingProperty>
            {
                new SortingProperty("", "Сортировать по"),
                new SortingProperty(nameof(BookResponse.Title), "Названию: А - Я"),
                new SortingProperty(nameof(BookResponse.Title), "Названию: Я - А"),
                new SortingProperty(nameof(BookResponse.RetailPrice), "Цене: мин. - макс."),
                new SortingProperty(nameof(BookResponse.RetailPrice), "Цене: макс. - мин.")
            };

            for (int i = 0; i < props.Count; i++)
            {
                var order = i % 2 == 0 ? nameof(SortingOrder.Desc) : nameof(SortingOrder.Asc);

                props[i].DescendingOrder = i % 2 == 0;

                var category = queryConditions.FilterConditions == null
                    ? 0.ToString()
                    : queryConditions.FilterConditions[0].PropertyValue;
                props[i].UrlLink = $"/{category}/Page{queryConditions.CurrentPage}/{props[i].PropertyName}/{order.ToUpper()}";
            }

            return props;
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