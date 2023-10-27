using SummitStories.API.Modules.Data.Interfaces;
using SummitStories.API.Modules.SqlDb.Interfaces;
using System.Data;

namespace SummitStories.API.Modules.Data.Repositories
{
    public class GenericRepository : IGenericRepository
    {
        protected ISqlDbService _sqlDbService;
        protected readonly ILogger<GenericRepository> _logger;

        public GenericRepository(ISqlDbService sqlDbService, ILogger<GenericRepository> logger)
        {
            _sqlDbService = sqlDbService;
            _logger = logger;
        }

        public virtual IList<IReadOnlyDictionary<string, dynamic>> Read(string queryString, IEnumerable<IDbDataParameter>? parameters = null)
        {
            List<IReadOnlyDictionary<string, dynamic>> results = new();

            _logger.LogTrace("Creating DB connection...");
            using IDbConnection dbConnection = _sqlDbService.CreateConnection();
            _logger.LogTrace("Creating DB command...");
            using IDbCommand command = dbConnection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = queryString;
            if (parameters != null)
            {
                foreach (IDbDataParameter param in parameters)
                {
                    command.Parameters.Add(param);
                }
            }
            _logger.LogTrace("Connecting to DB using connection string: {ConnectionString}...", dbConnection.ConnectionString);
            command.Connection!.Open();
            _logger.LogTrace("Executing DB command: {Command}...", queryString);
            using IDataReader reader = command.ExecuteReader();
            _logger.LogTrace("Reading DB result...");
            while (reader.Read())
            {
                object[] values = new object[reader.FieldCount];
                reader.GetValues(values);

                Dictionary<string, dynamic> result = new();
                foreach (int index in Enumerable.Range(0, reader.FieldCount))
                {
                    dynamic? value = values[index];
                    if (DBNull.Value.Equals(value))
                    {
                        value = null;
                    }
                    result.Add(reader.GetName(index), value);
                }
                results.Add(result);
            }

            return results;
        }


    }
}
