namespace BooksStore.Domain.Contracts.Models.Properties
{
    public class Property
    {
        public Property(string propertyName, string displayedName)
        {
            PropertyName = propertyName;
            DisplayedName = displayedName;
        }

        public string PropertyName { get; }
        public string DisplayedName { get; }
    }
}