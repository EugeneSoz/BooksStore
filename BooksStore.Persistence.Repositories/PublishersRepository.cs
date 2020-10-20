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
    public class PublishersRepository : IPublishersRepository
    {
        private readonly ConnectionProvider _connectionProvider;

        public PublishersRepository(ConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public (int count, IEnumerable<PublisherEntity> publishers) GetPublishers(SqlQueryConditions sqlQueryConditions)
        {
            var isSearchOrFilterUsed = !string.IsNullOrEmpty(sqlQueryConditions.WhereConditions);
            var sql = $@"SELECT * FROM Publishers{sqlQueryConditions}";

            var rowsCountSql = isSearchOrFilterUsed 
                ? $@"WITH Entities AS ({sql})
                     SELECT COUNT(*) AS Count
                       FROM Entities"
                : @"SELECT COUNT(*) FROM Publishers";

            using var connection = _connectionProvider.OpenConnection();
            var rowsCount = connection.ExecuteScalar<int>(rowsCountSql);
            var publishers = connection.Query<PublisherEntity>(sql);

            return (rowsCount, publishers);
        }

        public PublisherEntity GetPublisher(long id)
        {
            const string sql = @"SELECT P.*, B.Id, B.Authors, B.Title, B.PurchasePrice, B.RetailPrice
                          FROM Publishers AS P
                                   INNER JOIN Books AS B ON P.Id = B.PublisherId
                         WHERE P.Id = @id";
            var parameter = new DynamicParameters();
            parameter.Add("@id", id, DbType.Int64, ParameterDirection.Input);
            using (var connection = _connectionProvider.OpenConnection())
            {
                var publisherDictionary = new Dictionary<long, PublisherEntity>();
                var result = connection.Query<PublisherEntity, BookEntity, PublisherEntity>(
                    sql,
                    (publisher, book) =>
                    {
                        if (!publisherDictionary.TryGetValue(publisher.Id, out var publisherEntry))
                        {
                            publisherEntry = publisher;
                            publisherEntry.Books = new List<BookEntity>();
                            publisherDictionary.Add(publisherEntry.Id, publisherEntry);
                        }
                        publisherEntry.Books.Add(book);

                        return publisherEntry;
                    }, splitOn: nameof(BookEntity.Id), param: parameter).FirstOrDefault();

                return result;
            }
        }

        public PublisherEntity AddPublisher(PublisherEntity publisher)
        {
            const string sql = @"INSERT INTO Publishers (Name, Country)
                                 VALUES @name, @country";
            var parameters = new DynamicParameters();
            parameters.Add("@name", publisher.Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@country", publisher.Name, DbType.String, ParameterDirection.Input);
            using (var connection = _connectionProvider.OpenConnection())
            {
                var affectedRows = connection.Execute(sql, parameters);

                return affectedRows > 0 ? new PublisherEntity() : null;
            }
        }

        public bool UpdatePublisher(PublisherEntity publisher)
        {
            const string sql = @"UPDATE Publishers
                                 SET Name    = @name,
                                     Country = @country,
                                     Updated = GETDATE()";
            var parameters = new DynamicParameters();
            parameters.Add("@name", publisher.Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@country", publisher.Name, DbType.String, ParameterDirection.Input);
            using (var connection = _connectionProvider.OpenConnection())
            {
                var affectedRows = connection.Execute(sql, parameters);

                return affectedRows > 0;
            }
        }

        public bool DeletePublisher(PublisherEntity publisher)
        {
            const string sql = @"DELETE FROM Publishers
                                  WHERE Id = @id";
            var parameter = new DynamicParameters();
            parameter.Add("@id", publisher.Id, DbType.Int64, ParameterDirection.Input);
            using (var connection = _connectionProvider.OpenConnection())
            {
                var affectedRows = connection.Execute(sql, parameter);

                return affectedRows > 0;
            }
        }
    }
}