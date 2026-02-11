using Spotify_backend.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Spotify_backend.Services
{
    public class SpotifyGetInfo
    {
        public async Task<SpotifyProfile> GetProfile(string accessToken)
        {
            using var http = new HttpClient();

            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await http.GetAsync("https://api.spotify.com/v1/me");

            var json = await response.Content.ReadAsStringAsync();

            var profile = JsonSerializer.Deserialize<SpotifyProfile>(json);

            if (profile == null)
            {
                throw new InvalidOperationException("Failed to deserialize Spotify profile.");
            }

            return profile;
            
        }
    }
}
