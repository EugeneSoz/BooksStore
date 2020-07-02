using System.Collections.Generic;
using System.Data;
using System.Linq;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Repositories;
using BooksStore.Persistence.Entities;
using BooksStore.Persistence.Repositories.Providers;
using Dapper;

namespace BooksStore.Persistence.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly ConnectionProvider _connectionProvider;

        public CategoriesRepository(ConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public PagedList<CategoryEntity> GetCategories(PageOptions options)
        {
            if (string.IsNullOrEmpty(options.SortPropertyName))
            {
                options.SortPropertyName = nameof(CategoryEntity.Name);
            }
            if (options.SortPropertyName == $"{nameof(CategoryEntity.Name)}")
            {
                options.SortPropertyName = $"{nameof(CategoryEntity.ParentAndChildName)}";
            }

            const string rowsCountSql = @"SELECT COUNT(*) AS [Count]
                                   FROM Categories";
            var queryProcessing = new QueryProcessing<PageOptions>(options);
            var sql = $@"SELECT *
                          FROM Categories AS P
                                   LEFT JOIN Categories AS C ON P.Id = C.ParentId AND P.ParentId IS NULL
                         {queryProcessing.GetQueryConditions("P")}";
            using (var connection = _connectionProvider.OpenConnection())
            {
                var categoryDictionary = new Dictionary<long, CategoryEntity>();
                var rowsCount = connection.ExecuteScalar<int>(rowsCountSql);
                var result = connection.Query<CategoryEntity, CategoryEntity, CategoryEntity>(
                    sql,
                    (parent, child) =>
                    {
                        if (!categoryDictionary.TryGetValue(parent.Id, out var categoryEntry))
                        {
                            categoryEntry = parent;
                            categoryEntry.ParentAndChildName = child.ParentId == null
                                ? child.Name
                                : parent.Name + " <=> " + child.Name;
                            categoryDictionary.Add(categoryEntry.Id, categoryEntry);
                        }

                        return categoryEntry;
                    }, splitOn: nameof(CategoryEntity.Id)).ToList();

                return new PagedList<CategoryEntity>();
            }
        }

        public CategoryEntity GetCategory(long id)
        {
            const string sql = @"SELECT C.*, B.Id, B.Authors, B.Title, B.PurchasePrice, B.RetailPrice
                                   FROM Categories AS C
                                            INNER JOIN Books AS B ON C.Id = B.CategoryId
                                  WHERE C.Id = @id";
            var parameter = new DynamicParameters();
            parameter.Add("@id", id, DbType.Int64, ParameterDirection.Input);
            using (var connection = _connectionProvider.OpenConnection())
            {
                var categoryDictionary = new Dictionary<long, CategoryEntity>();
                var result = connection.Query<CategoryEntity, BookEntity, CategoryEntity>(
                    sql,
                    (category, book) =>
                    {
                        if (!categoryDictionary.TryGetValue(category.Id, out var categoryEntity))
                        {
                            categoryEntity = category;
                            categoryEntity.Books = new List<BookEntity>();
                            categoryDictionary.Add(categoryEntity.Id, categoryEntity);
                        }
                        categoryEntity.Books.Add(book);

                        return categoryEntity;
                    }, splitOn: nameof(BookEntity.Id), param: parameter).SingleOrDefault();

                return result;
            }
        }

        public List<CategoryEntity> GetStoreCategories()
        {
            const string sql = @"SELECT *
                                   FROM Categories AS P
                                            INNER JOIN Categories AS C ON P.Id = C.ParentId
                                  WHERE P.ParentId IS NULL
                                  ORDER BY P.Name, C.Name";
            using (var connection = _connectionProvider.OpenConnection())
            {
                var categoryDictionary = new Dictionary<long, CategoryEntity>();
                var result = connection.Query<CategoryEntity, CategoryEntity, CategoryEntity>(
                    sql,
                    (parent, child) =>
                    {
                        if (!categoryDictionary.TryGetValue(parent.Id, out var categoryEntry))
                        {
                            categoryEntry = parent;
                            categoryEntry.ChildrenCategories = new List<CategoryEntity>();
                            categoryDictionary.Add(categoryEntry.Id, categoryEntry);
                        }
                        categoryEntry.ChildrenCategories.Add(child);

                        return categoryEntry;
                    }, splitOn: nameof(CategoryEntity.Id)).ToList();

                return result;
            }
        }

        public List<CategoryEntity> GetParentCategories()
        {
            const string sql = @"SELECT *
                                   FROM Categories
                                  WHERE ParentId IS NULL
                                  ORDER BY Name";
            using (var connection = _connectionProvider.OpenConnection())
            {
                var categories = connection.Query<CategoryEntity>(sql).ToList();

                return categories;
            }
        }

        public CategoryEntity AddCategory(CategoryEntity category)
        {
            const string sql = @"INSERT INTO Categories (Name, ParentId)
                                 VALUES @name, @parentId";
            var parameters = new DynamicParameters();
            parameters.Add("@name", category.Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@parentId", category.ParentId, DbType.Int64, ParameterDirection.Input);
            using (var connection = _connectionProvider.OpenConnection())
            {
                var affectedRows = connection.Execute(sql, parameters);

                return affectedRows > 0 ? new CategoryEntity() : null;
            }
        }

        public bool UpdateCategory(CategoryEntity category)
        {
            const string sql = @"UPDATE Categories
                                    SET Name     = @name,
                                        ParentId = @parentId,
                                        Updated  = GETDATE()";
            var parameters = new DynamicParameters();
            parameters.Add("@name", category.Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@parentId", category.ParentId, DbType.Int64, ParameterDirection.Input);
            using (var connection = _connectionProvider.OpenConnection())
            {
                var affectedRows = connection.Execute(sql, parameters);

                return affectedRows > 0;
            }
        }

        public bool DeleteChildrenCategories(long parentId)
        {
            const string sql = @"DELETE FROM Categories
                                  WHERE ParentId = @id";
            var parameter = new DynamicParameters();
            parameter.Add("@id", parentId, DbType.Int64, ParameterDirection.Input);
            using (var connection = _connectionProvider.OpenConnection())
            {
                var affectedRows = connection.Execute(sql, parameter);

                return affectedRows > 0;
            }
        }

        public bool DeleteCategory(CategoryEntity category)
        {
            const string sql = @"DELETE FROM Categories
                                  WHERE Id = @id";
            var parameter = new DynamicParameters();
            parameter.Add("@id", category.Id, DbType.Int64, ParameterDirection.Input);
            using (var connection = _connectionProvider.OpenConnection())
            {
                var affectedRows = connection.Execute(sql, parameter);

                return affectedRows > 0;
            }
        }
    }
}