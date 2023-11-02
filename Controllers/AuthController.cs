//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using SummitStories.API.Modules.Strava.Repositories;

//namespace SummitStories.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
//        [HttpGet("start-auth")]
//        public IActionResult StartAuthorization()
//        {
//            // Tu dodaj kod do przygotowania parametrów autoryzacji, takich jak ClientId, RedirectUri itp.
//            // Następnie przekieruj użytkownika na Strava w celu wyrażenia zgody.
//            // Przykład:
//            var clientId = "YOUR_CLIENT_ID";
//            var redirectUri = "YOUR_REDIRECT_URI";
//            var stravaAuthUrl = $"https://www.strava.com/oauth/authorize?client_id={clientId}&redirect_uri={redirectUri}&response_type=code&scope=read,activity:read_all";
//            return Redirect(stravaAuthUrl);
//        }

//        [HttpGet("strava-callback")]
//        public async Task<IActionResult> HandleStravaCallback(string code)
//        {
//            try
//            {
//                // Utwórz instancję klasy StravaAuthorization
//                var stravaAuth = new StravaAuthorization(code);

//                // Wywołaj metodę do wymiany kodu na dostępowy token
//                var accessToken = await stravaAuth.ExchangeCodeForAccessTokenAsync();

//                // Tutaj możesz wykorzystać dostępowy token w swojej aplikacji
//                // np. zapisując go w bazie danych lub używając go do wykonywania operacji na Strava API.

//                // Zwróć komunikat JSON lub tekst do przeglądarki
//                return Ok("Autoryzacja zakończona pomyślnie!"); // Możesz dostosować komunikat do swoich potrzeb.
//            }
//            catch (Exception ex)
//            {
//                // Obsługa błędów
//                return BadRequest($"Błąd autoryzacji: {ex.Message}");
//            }
//        }
//    }
//}
