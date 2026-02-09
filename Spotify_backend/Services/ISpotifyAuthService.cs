namespace Spotify_backend.Services
{
    public interface ISpotifyAuthService
    {
        string GenerateLoginUrl(HttpContext context);
        Task<string> ExchangeCodeForToken(string code, string state);
    }
}
