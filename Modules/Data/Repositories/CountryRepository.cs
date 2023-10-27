﻿using Newtonsoft.Json;
using SummitStories.API.Modules.Data.Interfaces;
using SummitStories.API.Modules.Data.Models;
using SummitStories.API.Modules.SqlDb.Interfaces;

namespace SummitStories.API.Modules.Data.Repositories;

public class CountryRepository : GenericRepository, ICountryRepository
{
    public CountryRepository(ISqlDbService sqlDbService, ILogger<CountryRepository> logger) : base(sqlDbService, logger) { }

    public IList<Country> GetCountries()
    {
        IList<Country> countries = new List<Country>();
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

