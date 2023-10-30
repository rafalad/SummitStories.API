using System.Data;

namespace SummitStories.API.Modules.Data.Interfaces;

public interface IGenericRepository
{
    public IList<IReadOnlyDictionary<string, dynamic>> Read(string queryString, IEnumerable<IDbDataParameter>? parameters = null);
}