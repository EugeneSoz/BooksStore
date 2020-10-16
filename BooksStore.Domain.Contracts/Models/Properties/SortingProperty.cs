namespace BooksStore.Domain.Contracts.Models.Properties
{
    public class SortingProperty : Property
    {
        public SortingProperty(string propertyName, string displayedName) : base(propertyName, displayedName) { }

        public string UrlLink { get; set; }
        public bool? DescendingOrder { get; set; }
    }
}