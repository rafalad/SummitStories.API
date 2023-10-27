using Microsoft.Data.SqlClient;
using SummitStories.API.Modules.SqlDb.Interfaces;
using System.Data;

namespace SummitStories.API.Modules.SqlDb.Services
{
    public class SqlDbService : ISqlDbService
    {
        private readonly string _connectionString;

        public SqlDbService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public virtual IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
