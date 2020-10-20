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

        public (int count, IEnumerable<CategoryEntity> categries) GetCategories(SqlQueryConditions sqlQueryConditions)
        {
            var isSearchOrFilterUsed = !string.IsNullOrEmpty(sqlQueryConditions.WhereConditions);
            var sql = $@"SELECT * FROM Categories{sqlQueryConditions}";

            var rowsCountSql = isSearchOrFilterUsed 
                ? $@"WITH Entities AS ({sql})
                     SELECT COUNT(*) AS Count
                       FROM Entities"
                : @"SELECT COUNT(*) FROM Categories";

            using var connection = _connectionProvider.OpenConnection();
            var rowsCount = connection.ExecuteScalar<int>(rowsCountSql);
            var result = connection.Query<CategoryEntity>(sql);

            return (rowsCount, result);
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
            const string sql = @"SELECT * FROM Categories
                                  ORDER BY Name";
            using (var connection = _connectionProvider.OpenConnection())
            {
                var result = connection.Query<CategoryEntity>(
                    sql).Distinct().ToList();

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