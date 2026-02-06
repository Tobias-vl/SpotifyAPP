using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Web;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace Spotify_backend.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly string redirectUri = "http://127.0.0.1:5058/callback";

        [HttpGet("login")]
        public IActionResult Login()
        {

            var yaml = System.IO.File.ReadAllText("config.yaml");

            var deserializer = new DeserializerBuilder()
           .WithNamingConvention(CamelCaseNamingConvention.Instance)
           .Build();

            var config = deserializer.Deserialize<AppSettings>(yaml);

            string state = GenerateRandomString(16);
            string scope = "user-read-private user-read-email";
            HttpContext.Session.SetString("oauth_state", state);

            var query = HttpUtility.ParseQueryString(string.Empty);
            query["response_type"] = "code";
            query["client_id"] = config.app.clientId;
            query["scope"] = scope;
            query["redirect_uri"] = redirectUri;
            query["state"] = state;

            string url = "https://accounts.spotify.com/authorize?" + query.ToString();

            return Redirect(url);
        }


        [HttpGet("callback")]
        public async Task<IActionResult> Callback(string code, string state)
        {
            var storedState = HttpContext.Session.GetString("oauth_state");
            var yaml = System.IO.File.ReadAllText("config.yaml");

            var deserializer = new DeserializerBuilder()
           .WithNamingConvention(CamelCaseNamingConvention.Instance)
           .Build();

            var config = deserializer.Deserialize<AppSettings>(yaml);

            var clientSecret = config.app.clientSecret;

            if (storedState == null || storedState != state)
            {
                return BadRequest("Invalid state");
            }

            using var http = new HttpClient();
            var body = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        { "grant_type", "authorization_code" },
        { "code", code },
        { "redirect_uri", redirectUri },
        { "client_id", config.app.clientId },
        { "client_secret", clientSecret }
    });

            var response = await http.PostAsync("https://accounts.spotify.com/api/token", body);
            var json = await response.Content.ReadAsStringAsync();

            return Ok(json);
        }


        private string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);

            char[] result = new char[length];
            for (int i = 0; i < length; i++)
                result[i] = chars[bytes[i] % chars.Length];

            return new string(result);
        }
    }
}
