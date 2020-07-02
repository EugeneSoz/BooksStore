using System.Data.SqlClient;

namespace BooksStore.Persistence.Repositories.Providers
{
    public class ConnectionProvider
    {
        private readonly string _connectionString;

        public ConnectionProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection OpenConnection()
        {
            var connection = new SqlConnection(_connectionString);

            return connection;
        }
    }
}