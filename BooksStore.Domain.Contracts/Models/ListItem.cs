namespace OnlineBooksStore.Domain.Contracts.Models
{
    public class ListItem
    {
        public ListItem(string propertyName, string name, 
            bool descendingOrder = false, bool hasDivider = false, string href = "#")
        {
            PropertyName = propertyName;
            Name = name;
            DescendingOrder = descendingOrder;
            HasDivider = hasDivider;
            Href = href;
        }
        public string PropertyName { get; }
        public string Name { get; }
        public bool DescendingOrder { get; }
        public bool HasDivider { get; }
        public string Href { get; }
    }
}