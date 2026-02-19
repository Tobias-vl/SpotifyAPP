using Microsoft.AspNetCore.Mvc;
using Spotify_backend.Models;
using System.Net.Http.Headers;
using System.Text;
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
            var ListOfPlaylist = JsonSerializer.Deserialize<Playlist>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (ListOfPlaylist == null)
            {
                throw new Exception("Playlist list is empty");
            }

            return ListOfPlaylist;

        }


        public async Task<PlaylistItems> GetPlaylistItems(string playlist_id, string accessToken)
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _http.GetAsync($"https://api.spotify.com/v1/playlists/{playlist_id}/items");
            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Spotify API error ({response.StatusCode}): {error}");
            }
            var json = await response.Content.ReadAsStringAsync();
            var playlistItems = JsonSerializer.Deserialize<PlaylistItems>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (playlistItems == null)
            {
                throw new Exception("Playlist list is empty");
            }

            return playlistItems;

        }

        public async Task<string> DeletePlaylistItems(string playlist_id, string accessToken)
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _http.DeleteAsync($"https://api.spotify.com/v1/playlists/{playlist_id}/items");
            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Spotify API error ({response.StatusCode}): {error}");
            }
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> CreatePlaylist(string playlistName, string accessToken, string userId)
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var json = new
            {
                name = playlistName,

            };

            var content = new StringContent(
                JsonSerializer.Serialize(json),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _http.PostAsync(
                $"https://api.spotify.com/v1/users/{userId}/playlists",
                content
            );

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Spotify API error ({response.StatusCode}): {error}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            return responseJson;
        }




    }
}
