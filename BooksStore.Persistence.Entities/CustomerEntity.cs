namespace BooksStore.Persistence.Entities
{
    public class CustomerEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
    }
}