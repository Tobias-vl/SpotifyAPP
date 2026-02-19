using Spotify_backend.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Spotify_backend.Services
{
    public class SpotifyPlaylistService
    {
        private readonly SpotifyPlayerManager _playerManager;
        private readonly HttpClient _http;

        public SpotifyPlaylistService(SpotifyPlayerManager playerManager, HttpClient http)
        {
            _playerManager = playerManager;
            _http = http;
        }

        public async Task<Playlist> GetPlaylists(string accessToken, string userId)
        {
                       if (accessToken == null)
            {
                throw new Exception("accessToken is null");
            }

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _http.GetAsync($"https://api.spotify.com/v1/users/{userId}/playlists");
            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Spotify API error ({response.StatusCode}): {error}");
            }
            var json = await response.Content.ReadAsStringAsync();
            var ListOfPlaylist= JsonSerializer.Deserialize<Playlist>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (ListOfPlaylist == null)
            {
                throw new Exception("Playlist list is empty");
            }

            return ListOfPlaylist;

        }

    }
}
