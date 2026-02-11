using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.SignalR;

namespace Spotify_backend.Models
{
    public class SpotifyPlayer
    {
        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }
        public string? UserId { get; private set; }
        public DateTime ExpiresAt { get; private set; }

        public SpotifyPlayer(string accessToken, string refreshToken, DateTime expiresAt)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpiresAt = expiresAt;
        }

        public void SetUserId(string userId)
        {
            UserId = userId;
        }

        public void UpdateAccessToken(string accessToken)
        {
            AccessToken = accessToken;
        }

        public void UpdateTokens(string accessToken, string refreshToken, DateTime expiresAt)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpiresAt = expiresAt;
        }

    }

}
