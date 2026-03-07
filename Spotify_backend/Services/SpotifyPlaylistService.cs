using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Spotify_backend.Models;
using System.Collections.Immutable;
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

        public async Task<Playlists> GetPlaylists(string accessToken, string userId)
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
            var ListOfPlaylist = JsonSerializer.Deserialize<Playlists>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (ListOfPlaylist == null)
            {
                throw new Exception("Playlist list is empty");
            }

            return ListOfPlaylist;

        }


        public async Task<TrackItem> GetPlaylistItems(string accessToken, string playlist_id)
        {

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _http.GetAsync($"https://api.spotify.com/v1/playlists/{playlist_id}/tracks ");
            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Spotify API error ({response.StatusCode}): {error}");
            }
            var json = await response.Content.ReadAsStringAsync();
            var playlistItems = JsonSerializer.Deserialize<TrackItem>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (playlistItems == null)
            {
                throw new Exception("Playlist list is empty");
            }

            return playlistItems;

        }

        public async Task<CurrenttrackItem> GetCurrentTrack(string accessToken)
        {

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _http.GetAsync("https://api.spotify.com/v1/me/player/currently-playing");

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Spotify API error ({response.StatusCode}): {error}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var CurrentTrack = JsonSerializer.Deserialize<CurrenttrackItem>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return CurrentTrack;
        }

        public async Task<List<TrackItem>> GetTracks(string accessToken, string userId)
        {
            var listPlayist = await GetPlaylists(accessToken, userId);
            string playlistID = null;
            foreach (var playlist in listPlayist.Items)
            {
                if(playlist.Name == "On Repeat")
                {
                    playlistID = playlist.Id; break;
                }
            }

            if (playlistID == null)
            {
                throw new Exception("On repeat playlist not found");
            }

            var playlistTracks = await GetPlaylistItems(accessToken, playlistID);

            int number = 3;
            var trackItems = new List<TrackItem>();
            var usedID = new List<int>();

            var player = _playerManager.Get(userId) ?? throw new Exception("Player was null when trying to get tracks");
            playlistTracks.Track_owner = player.Name;

            var random = new Random();

            for (int i = 0; i < number; i++)
            {
                int id = random.Next(0, playlistTracks.Track_item.Count);
                if (!usedID.Contains(id))
                {
                    trackItems.Add(new TrackItem
                    {
                        Track_item = new List<Teack> { playlistTracks.Track_item[id] },
                        Track_owner = player.Name
                    });
                    usedID.Add(id);
                }
                else
                {
                    i--;
                }
            }
            return trackItems;
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
