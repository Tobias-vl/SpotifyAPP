using System.Web;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using System.Net.Http.Headers;
using System.Text.Json;
using Spotify_backend.Models;

namespace Spotify_backend.Services
{

    public class SpotifyAuthService : ISpotifyAuthService
    {

        
        private readonly StateGenerate _stateGenerate;
        private readonly SpotifyPlayerManager _playerManager;
        private readonly HttpClient _http;

        public SpotifyAuthService( StateGenerate stateGenerate, SpotifyPlayerManager playerManager, HttpClient http)
        {
           
            _stateGenerate = stateGenerate;
            _playerManager = playerManager;
            _http = http;
        }

        async Task<string> ISpotifyAuthService.ExchangeCodeForToken(string code, string state)
        {
            var yaml = System.IO.File.ReadAllText("config.yaml");

            var deserializer = new DeserializerBuilder()
           .WithNamingConvention(CamelCaseNamingConvention.Instance)
           .Build();

            var config = deserializer.Deserialize<AppSettings>(yaml);

            var tempPlayer = _playerManager.Get(state);
            if (tempPlayer == null)
            {
                throw new InvalidOperationException("Invalid state");
            }

            var body = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        { "grant_type", "authorization_code" },
        { "code", code },
        { "redirect_uri", "http://127.0.0.1:5058/callback" },
        { "client_id", config.app.clientId },
        { "client_secret", config.app.clientSecret }
    });

            var response = await _http.PostAsync("https://accounts.spotify.com/api/token", body);
            var json = await response.Content.ReadAsStringAsync();

            var tokenObj = System.Text.Json.JsonSerializer.Deserialize<SpotifyTokenResponse>(json); 

            var player = new SpotifyPlayer(tokenObj.access_token, tokenObj.refresh_token, DateTime.UtcNow.AddSeconds(tokenObj.expires_in));
            _playerManager.AddOrUpdate(state, player);

            return state;
        }

        string ISpotifyAuthService.GenerateLoginUrl(HttpContext context)
        {
            var yaml = System.IO.File.ReadAllText("config.yaml");

            var deserializer = new DeserializerBuilder()
           .WithNamingConvention(CamelCaseNamingConvention.Instance)
           .Build();

            var config = deserializer.Deserialize<AppSettings>(yaml);
            string state = _stateGenerate.GenerateRandomString(16);

            var tempplayer = new SpotifyPlayer("", "", DateTime.MinValue);
            _playerManager.AddOrUpdate(state, tempplayer);
            string scope = "user-read-private user-read-email playlist-read-private playlist-modify-public playlist-modify-private";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query["response_type"] = "code";
            query["client_id"] = config.app.clientId;
            query["scope"] = scope;
            query["redirect_uri"] = "http://127.0.0.1:5058/callback";
            query["state"] = state;

            string url = "https://accounts.spotify.com/authorize?" + query.ToString();

            return url;
        }

        async Task<string> ISpotifyAuthService.RenewToken(string userId)
        {

            var player = _playerManager.Get(userId);

            if (player == null)
            {
                throw new Exception("Player not found");
            }

            var refreshToken = player.RefreshToken;

            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new InvalidOperationException("No refresh token available in session.");
            }
                
            var yaml = System.IO.File.ReadAllText("config.yaml");

            var deserializer = new DeserializerBuilder()
           .WithNamingConvention(CamelCaseNamingConvention.Instance)
           .Build();
            var config = deserializer.Deserialize<AppSettings>(yaml);

            var body = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken },
                { "client_id", config.app.clientId },
                { "client_secret", config.app.clientSecret }
            });

            var response = await _http.PostAsync("https://accounts.spotify.com/api/token", body);
            var json = await response.Content.ReadAsStringAsync();

            var tokenObj = JsonSerializer.Deserialize<SpotifyTokenResponse>(json);

            player.UpdateTokens(
                accessToken: tokenObj.access_token,
                refreshToken: tokenObj.refresh_token ?? player.RefreshToken,
                expiresAt: DateTime.UtcNow.AddSeconds(tokenObj.expires_in)
            );

            return tokenObj.access_token;

        }
    }
}
