namespace BooksStore.App.Contracts.Query
{
    public abstract class CategoryQuery : Query { }
    public sealed class ParentCategoryCategoryQuery : CategoryQuery { }
    public sealed class StoreCategoryQuery : CategoryQuery { }
}