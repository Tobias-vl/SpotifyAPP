using Spotify_backend.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Spotify_backend.Services
{
    public class SpotifyGetInfo
    {
        private readonly HttpClient _http;
        private readonly SpotifyPlayerManager _playerManager;

        public SpotifyGetInfo(SpotifyPlayerManager playerManager, HttpClient http)
        {
            _playerManager = playerManager;
            _http = http;
        }

        public async Task<SpotifyProfile> GetProfile(string accessToken, string state)
        {

            if (accessToken == null)
            {
                throw new Exception("accessToken is null");
            }

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _http.GetAsync("https://api.spotify.com/v1/me");

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
            if (player == null)
            {
                throw new InvalidOperationException("Player don't exist");
            }
            player.SetUserId(profile.id);
            player.SetName(profile.display_name);

            Console.WriteLine($"{player.Name}");
            Console.WriteLine($"{player.UserId}");
            return profile;
        }


        //public async Task<SpotifyProfile> GetProfile(string accessToken, string state, CancellationToken cancellationToken = default)
        //{
        //    if (string.IsNullOrWhiteSpace(accessToken))
        //    {
        //        throw new Exception("accessToken is null or empty");
        //    }

        //    async Task<HttpResponseMessage> SendProfileRequest(string token)
        //    {
        //        using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.spotify.com/v1/me");
        //        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //        return await _http.SendAsync(request, cancellationToken).ConfigureAwait(false);
        //    }

        //    var response = await SendProfileRequest(accessToken).ConfigureAwait(false);
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //        {
        //            string newAccessToken = await _spotify.RenewToken(state).ConfigureAwait(false);

        //            response = await SendProfileRequest(newAccessToken).ConfigureAwait(false);
        //        }

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            string error = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        //            throw new Exception($"Spotify API error ({response.StatusCode}): {error}");
        //        }
        //    }

        //    var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        //    var profile = JsonSerializer.Deserialize<SpotifyProfile>(json, new JsonSerializerOptions
        //    {
        //        PropertyNameCaseInsensitive = true
        //    });

        //    if (profile == null)
        //    {
        //        throw new InvalidOperationException("Failed to deserialize Spotify profile.");
        //    }

        //    _playerManager.ReplaceKey(state, profile.id);

        //    var player = _playerManager.Get(profile.id);
        //    if (player == null)
        //    {
        //        throw new InvalidOperationException("Player does not exist after ReplaceKey");
        //    }

        //    player.SetName(profile.display_name);
        //    return profile;
        //}

    }
}
