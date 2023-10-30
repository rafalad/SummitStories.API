using SummitStories.API.Modules.Data.Models;

namespace SummitStories.API.Modules.Data.Interfaces;

public interface ICountryRepository : IGenericRepository
{
    public IList<Country> GetCountries();
}