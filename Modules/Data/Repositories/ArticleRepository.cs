using Newtonsoft.Json;
using SummitStories.API.Modules.Data.Interfaces;
using SummitStories.API.Modules.Data.Models;
using SummitStories.API.Modules.SqlDb.Interfaces;

namespace SummitStories.API.Modules.Data.Repositories;

public class ArticleRepository : GenericRepository, IArticleRepository
{
    public ArticleRepository(ISqlDbService sqlDbService, ILogger<ArticleRepository> logger) : base(sqlDbService, logger) { }

    public IList<Article> GetCountries()
    {
        IList<Article> countries = new List<Article>();
        string queryString =
            "SELECT *\n" +
            "FROM dbo.Countries";

        IList<IReadOnlyDictionary<string, dynamic>> results = Read(queryString);
        _logger.LogTrace("Raw result: {RawResult}", JsonConvert.SerializeObject(results));
        foreach (IReadOnlyDictionary<string, dynamic> result in results)
        {
            countries.Add(new()
            {
                Name = result.GetValueOrDefault("country", ""),
                NamePolish = result.GetValueOrDefault("namepolish", "")
            });
        }
        _logger.LogTrace("Result: {Result}", JsonConvert.SerializeObject(countries));

        return countries;
    }
}