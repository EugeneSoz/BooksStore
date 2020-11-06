namespace BooksStore.Domain.Contracts.Models.Pages
{
    public class PageOptions
    {
        public long Category { get; set; }
        public int Page { get; set; }
        public string PropertyName { get; set; }
        public string Order { get; set; }
    }
}