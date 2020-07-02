namespace BooksStore.App.Contracts.Query
{
    public class PageFilterQuery : Query
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortPropertyName { get; set; }
        //сортировка по убыванию
        public bool DescendingOrder { get; set; }
        //по какому св-ву поиск
        public string[] SearchPropertyNames { get; set; }
        //что ищем
        public string SearchTerm { get; set; }
        //по какому св-ву фильтруем
        public string FilterPropertyName { get; set; }
        //id категории книги
        public long FilterPropertyValue { get; set; }
    }
}