namespace OnlineBooksStore.App.Contracts.Query
{
    public abstract class CategoryQuery : BooksStore.App.Contracts.Query.Query { }
    public sealed class ParentCategoryCategoryQuery : CategoryQuery { }
    public sealed class StoreCategoryQuery : CategoryQuery { }
}