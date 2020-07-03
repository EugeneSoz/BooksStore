namespace BooksStore.Domain.Contracts.Models
{
    public class FilterSortingProps
    {
        public FilterSortingProps(string propertyName, string displayedName)
        {
            PropertyName = propertyName;
            DisplayedName = displayedName;
        }

        public string PropertyName { get; }
        public string DisplayedName { get; }
    }
}
