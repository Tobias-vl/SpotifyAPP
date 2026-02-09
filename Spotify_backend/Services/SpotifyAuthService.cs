using System.Web;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Spotify_backend.Services
{

    public class SpotifyAuthService : ISpotifyAuthService
    {

        private readonly IHttpContextAccessor _accessor;
        private readonly StateGenerate _stateGenerate;

        public SpotifyAuthService(IHttpContextAccessor accessor, StateGenerate stateGenerate)
        {
            _accessor = accessor;
            _stateGenerate = stateGenerate;
        }

        async Task<string> ISpotifyAuthService.ExchangeCodeForToken(string code, string state)
        {
            var storedState = _accessor.HttpContext.Session.GetString("oauth_state");
            var yaml = System.IO.File.ReadAllText("config.yaml");

            var deserializer = new DeserializerBuilder()
           .WithNamingConvention(CamelCaseNamingConvention.Instance)
           .Build();

            var config = deserializer.Deserialize<AppSettings>(yaml);

            var clientSecret = config.app.clientSecret;
            //todo: fix
            //if (storedState == null || storedState != state)
            //{
            //    return BadRequest("Invalid state");
            //}

            using var http = new HttpClient();
            var body = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        { "grant_type", "authorization_code" },
        { "code", code },
        { "redirect_uri", "http://127.0.0.1:5058/callback" },
        { "client_id", config.app.clientId },
        { "client_secret", clientSecret }
    });

            var response = await http.PostAsync("https://accounts.spotify.com/api/token", body);
            var json = await response.Content.ReadAsStringAsync();

            var tokenObj = System.Text.Json.JsonSerializer.Deserialize<SpotifyTokenResponse>(json);

            _accessor.HttpContext.Session.SetString("spotify_access_token", tokenObj.access_token);
            _accessor.HttpContext.Session.SetString("spotify_refresh_token", tokenObj.refresh_token);

            return json;
        }

        string ISpotifyAuthService.GenerateLoginUrl(HttpContext context)
        {
            var yaml = System.IO.File.ReadAllText("config.yaml");

            var deserializer = new DeserializerBuilder()
           .WithNamingConvention(CamelCaseNamingConvention.Instance)
           .Build();

            var config = deserializer.Deserialize<AppSettings>(yaml);
            string state = _stateGenerate.GenerateRandomString(16);
            string scope = "user-read-private user-read-email playlist-read-private ";
            _accessor.HttpContext.Session.SetString("oauth_state", state);

            var query = HttpUtility.ParseQueryString(string.Empty);
            query["response_type"] = "code";
            query["client_id"] = config.app.clientId;
            query["scope"] = scope;
            query["redirect_uri"] = "http://127.0.0.1:5058/callback";
            query["state"] = state;

            string url = "https://accounts.spotify.com/authorize?" + query.ToString();

            return url;
        }

        async Task<string> ISpotifyAuthService.RenewToken()
        {
            var refreshToken = "AQAKy-exehSKGKyx4803JLLS9qsg0-Q8TnrC5eLDgQkY6scLPNKYpD_HKJiSxXseVX_ksSNr1yDMYvje6PzESJoN3CGPJNwuKbW6lc7zYrgPK0wRk6rZAubmPmOK20NrsCE";

            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new InvalidOperationException("No refresh token available in session.");
            }
                
            var yaml = System.IO.File.ReadAllText("config.yaml");

            var deserializer = new DeserializerBuilder()
           .WithNamingConvention(CamelCaseNamingConvention.Instance)
           .Build();
            var config = deserializer.Deserialize<AppSettings>(yaml);

            using var http = new HttpClient();
            var body = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken },
                { "client_id", config.app.clientId },
                { "client_secret", config.app.clientSecret }
            });

            var response = await http.PostAsync("https://accounts.spotify.com/api/token", body);
            var json = await response.Content.ReadAsStringAsync();

            var tokenObj = JsonSerializer.Deserialize<SpotifyTokenResponse>(json);

            _accessor.HttpContext.Session.SetString("spotify_access_token", tokenObj.access_token);

            if (!string.IsNullOrEmpty(tokenObj.refresh_token))
            {
                _accessor.HttpContext.Session.SetString("spotify_refresh_token", tokenObj.refresh_token);
            }

            return tokenObj.access_token;

        }
    }
}
