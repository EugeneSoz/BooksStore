using System;

namespace BooksStore.Domain.Contracts.Models.Pages
{
    /// <summary>
    /// Класс для хранения информации об описании условий sql запроса
    /// </summary>
    public class Condition
    {
        public Condition(string propertyName, string propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
            Alias = string.Empty;
        }

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
        public string PropertyValue { get; }
        /// <summary>
        /// Псевдоним для свойства (C.Name, например).
        /// </summary>
        /// <value>
        /// Псевдоним для свойства.
        /// </value>
        public string Alias { get; set; }
    }
}