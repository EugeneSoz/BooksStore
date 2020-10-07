namespace BooksStore.Domain.Contracts.Models.Pages
{
    /// <summary>
    /// Класс хранит условия, которые будут использоваться для извлечения данных из бд
    /// </summary>
    public class QueryConditions
    {
        /// <summary>
        /// Текущая выбранная страница.
        /// </summary>
        /// <value>
        /// Текущая выбранная страница.
        /// </value>
        public int CurrentPage { get; set; }
        /// <summary>
        /// Количество отображаемых страниц.
        /// </summary>
        /// <value>
        /// Количество отображаемых страниц.
        /// </value>
        public int PageSize { get; set; }
        /// <summary>
        /// Условия фильтрации.
        /// </summary>
        /// <value>
        /// Условия фильтрации.
        /// </value>
        public Condition[] FilterConditions { get; set; }

        /// <summary>
        /// Условия поиска.
        /// </summary>
        /// <value>
        /// Условия поиска.
        /// </value>
        public Condition[] SearchConditions { get; set; }
        /// <summary>
        /// Условия сортировки.
        /// </summary>
        /// <value>
        /// Условия сортировки.
        /// </value>
        public Condition[] OrderConditions { get; set; }
    }
}