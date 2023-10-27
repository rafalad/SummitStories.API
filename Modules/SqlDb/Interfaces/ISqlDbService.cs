using System.Data;

namespace SummitStories.API.Modules.SqlDb.Interfaces
{
    public interface ISqlDbService
    {
        IDbConnection CreateConnection();
    }
}
