using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Models.Publishers;
using BooksStore.Domain.Contracts.Services;
using OnlineBooksStore.Domain.Contracts.Models.Categories;

namespace BooksStore.Domain.Services
{
    public class PropertiesService : IPropertiesService
    {
        public List<FilterSortingProps> GetPublisherFilterProps()
        {
            return new List<FilterSortingProps>
            {
                new FilterSortingProps(nameof(Publisher.Name), "Издательство"),
                new FilterSortingProps(nameof(Publisher.Country), "Страна нахождения")
            };
        }

        public List<FilterSortingProps> GetCategoryFilterProps()
        {
            return new List<FilterSortingProps>
            {
                new FilterSortingProps(nameof(Category.Name), "Категория"),
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

        public List<FilterSortingProps> GetPublisherSortingProps()
        {
            return new List<FilterSortingProps>
            {
                new FilterSortingProps(nameof(Publisher.Id), "ID"),
                new FilterSortingProps(nameof(Publisher.Name), "Издательство"),
                new FilterSortingProps(nameof(Publisher.Country), "Страна нахождения издательства")
            };
        }

        public List<FilterSortingProps> GetCategorySortingProps()
        {
            return new List<FilterSortingProps>
            {
                new FilterSortingProps(nameof(Category.Id), "ID"),
                new FilterSortingProps(nameof(Category.Name), "Категория")
            };
        }

        public List<FilterSortingProps> GetBooksSortingProps()
        {
            return new List<FilterSortingProps>
            {
                new FilterSortingProps(nameof(BookResponse.Id), "ID"),
                new FilterSortingProps(nameof(BookResponse.Title), "Название"),
                new FilterSortingProps(nameof(BookResponse.CategoryName), "Категория"),
                new FilterSortingProps(nameof(BookResponse.SubcategoryName), "Подкатегория"),
                new FilterSortingProps(nameof(BookResponse.PublisherName), "Издательство"),
                new FilterSortingProps(nameof(BookResponse.RetailPrice), "Цена")
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