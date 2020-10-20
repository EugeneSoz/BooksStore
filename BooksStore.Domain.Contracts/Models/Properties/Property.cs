using System.Collections.Generic;

namespace BooksStore.Domain.Contracts.Models.Properties
{
    public class Property
    {
        protected Property(string propertyName, string displayedName)
        {
            (PropertyName, DisplayedName) = (propertyName, displayedName);
        }

        public string PropertyName { get; }
        public string DisplayedName { get; }
    }
}