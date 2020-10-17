namespace BooksStore.Domain.Contracts.Models.Properties
{
    public class FilterProperty : Property
    {
        public FilterProperty(string propertyName, string displayedName, bool isActive) : base(propertyName, displayedName)
        {
            IsActive = isActive;
        }

        public bool IsActive { get; }
    }
}