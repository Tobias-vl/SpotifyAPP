using Microsoft.OpenApi.Any;
using Spotify_backend.Models;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Spotify_backend.Services
{
    public class SpotifyGetInfo
    {

        private readonly SpotifyPlayerManager _playerManager;

        public SpotifyGetInfo(SpotifyPlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        public async Task<SpotifyProfile> GetProfile(string accessToken, string state)
        {

            if (accessToken == null)
            {
                throw new Exception("accessToken is null");
            }
            using var http = new HttpClient();

            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await http.GetAsync("https://api.spotify.com/v1/me");

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Spotify API error ({response.StatusCode}): {error}");
            }

            var json = await response.Content.ReadAsStringAsync();

            var profile = JsonSerializer.Deserialize<SpotifyProfile>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (profile == null)
            {
                throw new InvalidOperationException("Failed to deserialize Spotify profile.");
            }

            _playerManager.ReplaceKey(state, profile.id);
            var player = _playerManager.Get(profile.id);
            Console.WriteLine($"{player.UserId}");
            Console.WriteLine($"{profile.id}");

            if (player == null)
            {
                throw new InvalidOperationException("Player don't exist");
            }

            player.SetName(profile.display_name);
            Console.WriteLine($"{profile.display_name}");
            return profile;
        }
    }
}
