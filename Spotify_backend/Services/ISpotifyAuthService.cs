using System.Reflection.Metadata;

namespace Spotify_backend.Services
{
    public interface ISpotifyAuthService
    {
        string GenerateLoginUrl(HttpContext context);
        Task<string> ExchangeCodeForToken(string code, string state);

        Task<string> RenewToken(string UserId);
    }
}
