namespace BooksStore.Domain.Contracts.Models.Pages
{
    /// <summary>
    /// Класс для хранения информации об описании условий sql запроса
    /// </summary>
    public class Condition
    {
        /// <summary>
        /// Имя свойства.
        /// </summary>
        /// <value>
        /// Имя свойства.
        /// </value>
        public string PropertyName { get; set; }
        /// <summary>
        /// Значение свойства.
        /// </summary>
        /// <value>
        /// Значение свойства.
        /// </value>
        public string PropertyValue { get; set; }
    }
}