using Spotify_backend.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Spotify_backend.Services
{
    public class MediaPlayer
    {

        private readonly HttpClient _http;

        public MediaPlayer(HttpClient http)
        {
            _http = http;
        }

        public async Task<Device> Getdevice(string AccessToken)
        {

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var response = await _http.GetAsync("https://api.spotify.com/v1/me/player/devices");


            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Spotify API error ({response.StatusCode}): {error}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var device = JsonSerializer.Deserialize<Device>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (device == null ) { throw new Exception("device not found"); }

            return device;
        }
        public async Task Pause(string deviceId, string accessToken)
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var url = $"https://api.spotify.com/v1/me/player/pause?device_id={deviceId}";

            var response = await _http.PutAsync(url, null);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Spotify API error ({response.StatusCode}): {error}");
            }
        }

        public async Task Resume(string  deviceId, string accessToken)
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var url = $"https://api.spotify.com/v1/me/player/play?device_id={deviceId}";

            var response = await _http.PutAsync(url, null);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Spotify API error ({response.StatusCode}): {error}");
            }
        }

        public async Task Skip(string accessToken)
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var url = $"https://api.spotify.com/v1/me/player/next";

            var response = await _http.PostAsync(url, null);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Spotify API error ({response.StatusCode}): {error}");
            }
        }

        public async Task Repeat(string accessToken)
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var url = $"https://api.spotify.com/v1/me/player/repeat?state=track";

            var response = await _http.PutAsync(url, null);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Spotify API error ({response.StatusCode}): {error}");
            }
        }

    }
}
