namespace OnlineBooksStore.Domain.Contracts.Models.Categories
{
    public class CategoryResponse : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        //если свойство не равно null, тогда категория является подкатегорией
        public long? ParentId { get; set; }
        public string ParentCategoryName { get; set; }
        public string DisplayedName { get; set; }
    }
}
