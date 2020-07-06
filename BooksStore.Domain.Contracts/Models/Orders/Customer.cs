namespace BooksStore.Domain.Contracts.Models.Orders
{
    public class Customer : EntityBase
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}