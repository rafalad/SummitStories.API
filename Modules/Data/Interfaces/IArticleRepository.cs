using SummitStories.API.Modules.Data.Models;

namespace SummitStories.API.Modules.Data.Interfaces;

public interface IArticleRepository : IGenericRepository
{
    public IList<Article> GetCountries();
}