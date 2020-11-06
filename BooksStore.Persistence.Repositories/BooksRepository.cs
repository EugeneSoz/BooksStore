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
    public class BooksRepository : IBooksRepository
    {
        private readonly ConnectionProvider _connectionProvider;

        public BooksRepository(ConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public (int count, IEnumerable<BookEntity> books) GetBooks(SqlQueryConditions sqlQueryConditions)
        {
            var isSearchOrFilterUsed = !string.IsNullOrEmpty(sqlQueryConditions.WhereConditions);
            var sql = $@"SELECT B.*, C.*, P.*
                                  FROM Books AS B
                                           INNER JOIN Categories AS C ON B.CategoryId = C.Id
                                           INNER JOIN Publishers AS P ON B.PublisherId = P.Id{sqlQueryConditions}";

            var rowsCountSql = isSearchOrFilterUsed 
                ? $@"WITH Entities AS (
                        SELECT B.Id
                          FROM Books AS B
                          INNER JOIN Categories AS C ON B.CategoryId = C.Id
                          INNER JOIN Publishers AS P ON B.PublisherId = P.Id{sqlQueryConditions})
                     SELECT COUNT(*) AS Count
                       FROM Entities"
                : @"SELECT COUNT(*) AS Count FROM Books";

            using var connection = _connectionProvider.OpenConnection();
            var rowsCount = connection.ExecuteScalar<int>(rowsCountSql);
            var result = connection.Query<BookEntity, CategoryEntity, PublisherEntity, BookEntity>(
                sql,
                (book, category, publisher) =>
                {
                    book.Category = category;
                    book.Publisher = publisher;

                    return book;
                }, splitOn: nameof(BaseEntity.Id));

            return (rowsCount, result);
        }

        public IEnumerable<BookEntity> GetBooksByIds(IEnumerable<long> booksIds)
        {
            const string sql = @"SELECT *
                                   FROM Books
                                  WHERE Id IN @ids";
            using (var connection = _connectionProvider.OpenConnection())
            {
                var books = connection.Query<BookEntity>(sql, new {ids = booksIds});

                return books;
            }
        }

        public BookEntity GetBook(long key)
        {
            const string sql = @"SELECT B.*, C.*, P.*
                                  FROM Books AS B
                                           INNER JOIN Categories AS C ON B.CategoryId = C.Id
                                           INNER JOIN Publishers AS P ON B.PublisherId = P.Id
                                WHERE B.Id = @id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", key, DbType.Int64, ParameterDirection.Input);
            using (var connection = _connectionProvider.OpenConnection())
            {
                var result = connection.Query<BookEntity, CategoryEntity, PublisherEntity, BookEntity>(
                    sql,
                    (book, category, publisher) =>
                    {
                        book.Category = category;
                        book.Publisher = publisher;

                        return book;
                    }, splitOn: nameof(BookEntity.Id), param: parameters).SingleOrDefault();

                return result;
            }
        }

        public BookEntity AddBook(BookEntity book)
        {
            const string sql =
                @"INSERT INTO Books (Title, Authors, Year, Language, PageCount, Description, PurchasePrice, RetailPrice, CategoryId, PublisherId)
                  VALUES @title, @authors, @year, @language, @pageCount, @description, @purchasePrice, @retailPrice, @categoryId, @publisherId";
            var parameters = new DynamicParameters();
            parameters.Add("@name", book.Title, DbType.String, ParameterDirection.Input);
            parameters.Add("@authors", book.Authors, DbType.String, ParameterDirection.Input);
            parameters.Add("@year", book.Year, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@language", book.Language, DbType.String, ParameterDirection.Input);
            parameters.Add("@pageCount", book.PageCount, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@description", book.Description, DbType.String, ParameterDirection.Input);
            parameters.Add("@purchasePrice", book.PurchasePrice, DbType.Decimal, ParameterDirection.Input);
            parameters.Add("@retailPrice", book.RetailPrice, DbType.Decimal, ParameterDirection.Input);
            parameters.Add("@categoryId", book.PublisherId, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@publisherId", book.CategoryId, DbType.Int64, ParameterDirection.Input);
            using (var connection = _connectionProvider.OpenConnection())
            {
                var affectedRows = connection.Execute(sql, parameters);

                return affectedRows > 0 ? new BookEntity() : null;
            }
        }

        public bool UpdateBook(BookEntity book)
        {
            const string sql = @"UPDATE Books
                                   SET Title         = @title,
                                       Authors       = @authors,
                                       Year          = @year,
                                       Language      = @language,
                                       PageCount     = @pageCount,
                                       Description   = @description,
                                       PurchasePrice = @purchasePrice,
                                       RetailPrice   = @retailPrice,
                                       CategoryId    = @categoryId,
                                       PublisherId   = @publisherId,
                                       Updated       = GETDATE()";
            var parameters = new DynamicParameters();
            parameters.Add("@name", book.Title, DbType.String, ParameterDirection.Input);
            parameters.Add("@authors", book.Authors, DbType.String, ParameterDirection.Input);
            parameters.Add("@year", book.Year, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@language", book.Language, DbType.String, ParameterDirection.Input);
            parameters.Add("@pageCount", book.PageCount, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@description", book.Description, DbType.String, ParameterDirection.Input);
            parameters.Add("@purchasePrice", book.PurchasePrice, DbType.Decimal, ParameterDirection.Input);
            parameters.Add("@retailPrice", book.RetailPrice, DbType.Decimal, ParameterDirection.Input);
            parameters.Add("@categoryId", book.PublisherId, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@publisherId", book.CategoryId, DbType.Int64, ParameterDirection.Input);
            using (var connection = _connectionProvider.OpenConnection())
            {
                var affectedRows = connection.Execute(sql, parameters);

                return affectedRows > 0;
            }
        }

        public bool Delete(BookEntity book)
        {
            const string sql = @"DELETE FROM Books
                                  WHERE Id = @id";
            var parameter = new DynamicParameters();
            parameter.Add("@id", book.Id, DbType.Int64, ParameterDirection.Input);
            using (var connection = _connectionProvider.OpenConnection())
            {
                var affectedRows = connection.Execute(sql, parameter);

                return affectedRows > 0;
            }
        }
    }
}