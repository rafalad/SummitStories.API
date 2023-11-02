//namespace SummitStories.API.Modules.Strava.Repositories
//{
//    public class StravaAuthorization
//    {
//        private readonly string clientId = "YOUR_CLIENT_ID";
//        private readonly string clientSecret = "YOUR_CLIENT_SECRET";
//        private readonly string redirectUri = "YOUR_REDIRECT_URI";
//        private readonly string code; // Kod autoryzacji uzyskany z przekierowania z Strava

//        public StravaAuthorization(string code)
//        {
//            this.code = code;
//        }

//        public async Task<string> ExchangeCodeForAccessTokenAsync()
//        {
//            using var httpClient = new HttpClient();

//            var tokenUrl = "https://www.strava.com/oauth/token";
//            var content = new FormUrlEncodedContent(new Dictionary<string, string>
//        {
//            {"client_id", clientId},
//            {"client_secret", clientSecret},
//            {"code", code},
//            {"grant_type", "authorization_code"},
//            {"redirect_uri", redirectUri}
//        });

//            var response = await httpClient.PostAsync(tokenUrl, content);

//            if (response.IsSuccessStatusCode)
//            {
//                var responseContent = await response.Content.ReadAsStringAsync();
//                // Deserializuj odpowiedź JSON i wyodrębnij dostępowy token
//                // Przykład użycia biblioteki Newtonsoft.Json:
//                // var token = JsonConvert.DeserializeObject<StravaAccessToken>(responseContent);
//                // return token.AccessToken;
//                return responseContent;
//            }
//            else
//            {
//                // Obsługa błędów w przypadku niepowodzenia uzyskania tokena
//                throw new Exception("Błąd podczas wymiany kodu na dostępowy token");
//            }
//        }
//    }
//}
