//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using SummitStories.API.Modules.Strava.Interfaces;

//namespace SummitStories.API.Controllers
//{
//    [Route("api")]
//    [ApiController]
//    public class StravaController : ControllerBase
//    {
//        [HttpGet("activity/{id}")]
//        public IActionResult GetActivityById(long id)
//        {
//            try
//            {
//                // Tutaj skonfiguruj dostępowy token OAuth2
//                Configuration.Default.AccessToken = "YOUR_ACCESS_TOKEN";

//                var apiInstance = new ActivitiesApi();
//                var includeAllEfforts = true;  // Opcjonalnie: aby uwzględnić wszystkie wysiłki na segmencie

//                // Wywołaj metodę, aby pobrać szczegółowe informacje o aktywności
//                DetailedActivity result = apiInstance.getActivityById(id, includeAllEfforts);

//                // Możesz zwrócić te szczegółowe informacje jako wynik
//                return Ok(result);
//            }
//            catch (Exception e)
//            {
//                // Obsługa błędów
//                return BadRequest("Błąd podczas pobierania aktywności z API Strava: " + e.Message);
//            }
//        }
//    }
//}
