using Spotify_backend.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Spotify_backend.Services
{
    public class SpotifyGetInfo
    {
        private readonly HttpClient _http;
        private readonly SpotifyPlayerManager _playerManager;
        private readonly ISpotifyAuthService _spotify;

        public SpotifyGetInfo(SpotifyPlayerManager playerManager, HttpClient http, ISpotifyAuthService spotify)
        {
            _playerManager = playerManager;
            _http = http;
            _spotify = spotify;
        }
        public async Task<SpotifyProfile> GetProfile(string accessToken, string state, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new Exception("accessToken is null or empty");
            }

            // Build request with per-request Authorization header (do not mutate DefaultRequestHeaders)
            async Task<HttpResponseMessage> SendProfileRequest(string token)
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.spotify.com/v1/me");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await _http.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }

            var response = await SendProfileRequest(accessToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                // If token expired (401), try refresh once then retry
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // RenewToken expects the manager key (state or userId). Pass the same key used to locate the player.
                    string newAccessToken = await _spotify.RenewToken(state).ConfigureAwait(false);

                    response = await SendProfileRequest(newAccessToken).ConfigureAwait(false);
                }

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                    throw new Exception($"Spotify API error ({response.StatusCode}): {error}");
                }
            }

            var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            var profile = JsonSerializer.Deserialize<SpotifyProfile>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (profile == null)
            {
                throw new InvalidOperationException("Failed to deserialize Spotify profile.");
            }

            // Replace the temporary state key with the real user id (atomic manager operation expected)
            _playerManager.ReplaceKey(state, profile.id);

            var player = _playerManager.Get(profile.id);
            if (player == null)
            {
                throw new InvalidOperationException("Player does not exist after ReplaceKey");
            }

            player.SetName(profile.display_name);
            return profile;
        }
    }
}
