//using Microsoft.Extensions.Configuration;
//using SummitStories.API.Modules.Strava.Interfaces;
//using System.Diagnostics;
//using System;
//using System.Diagnostics;
//using com.strava.api.v3.Api;
//using com.strava.api.v3.Client;
//using com.strava.api.v3.Model;

//namespace SummitStories.API.Modules.Strava.Repositories
//{
//    public class StravaClient : IStravaClient
//    {
//        private readonly ILogger<StravaClient> _logger;
//        private readonly IConfiguration _configuration;
//        private readonly string _stravaAccessToken;

//        public StravaClient(ILogger<StravaClient> logger, IConfiguration configuration, string stravaAccessToken)
//        {
//            _logger = logger;
//            _configuration = configuration;
//            _stravaAccessToken = stravaAccessToken;
//        }

//        public async Task<DetailedActivity> GetActivityByIdAsync(long id)
//        {
//            try
//            {
//                // Konfiguracja OAuth2 - przypisanie tokena dostępowego
//                Configuration.Default.AccessToken = _stravaAccessToken;

//                var apiInstance = new ActivitiesApi();
//                var includeAllEfforts = true;

//                // Pobranie aktywności na podstawie identyfikatora
//                DetailedActivity result = await apiInstance.getActivityByIdAsync(id, includeAllEfforts);
//                _logger.LogInformation("Pobrano aktywność z Strava: {0}", result.Name);

//                return result;
//            }
//            catch (Exception e)
//            {
//                _logger.LogError("Błąd podczas pobierania aktywności z Strava: {0}", e.Message);
//                throw;
//            }
//        }
//    }
//}
