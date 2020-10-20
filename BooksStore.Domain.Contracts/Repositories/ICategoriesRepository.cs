using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Persistence.Entities;

namespace BooksStore.Domain.Contracts.Repositories
{
    public interface ICategoriesRepository
    {
        (int count, IEnumerable<CategoryEntity> categries) GetCategories(SqlQueryConditions sqlQueryConditions);
        CategoryEntity GetCategory(long id);
        List<CategoryEntity> GetStoreCategories();
        List<CategoryEntity> GetParentCategories();
        CategoryEntity AddCategory(CategoryEntity category);
        bool UpdateCategory(CategoryEntity category);
        bool DeleteChildrenCategories(long parentId);
        bool DeleteCategory(CategoryEntity category);
    }
}
