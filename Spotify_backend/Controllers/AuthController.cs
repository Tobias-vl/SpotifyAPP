using Microsoft.AspNetCore.Mvc;
using Spotify_backend.Services;

namespace Spotify_backend.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly ISpotifyAuthService _spotify;
        private readonly SpotifyGetInfo _GetSpotify;
        private readonly SpotifyPlayerManager _playerManager;

        public AuthController(ISpotifyAuthService spotify, SpotifyGetInfo GetSpotify, SpotifyPlayerManager playerManager)
        {
            _spotify = spotify;
            _GetSpotify = GetSpotify;
            _playerManager = playerManager;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            string url = _spotify.GenerateLoginUrl(HttpContext);

            return Redirect(url);
        }


        [HttpGet("callback")]
        public async Task<IActionResult> Callback(string code, string state)
        {
            
            var tokenObj = await _spotify.ExchangeCodeForToken(code, state);

            var player = _playerManager.Get(state) ?? throw new Exception("Player was Null");
            var profile = await _GetSpotify.GetProfile(player.AccessToken, state);

            player.SetName(profile.display_name);

            return Redirect("http://localhost:3000/lobby");
        }

        [HttpPost("RenewToken")]

        public async Task<IActionResult> RenewToken(string UserId)
        {
            var tokenObj = await _spotify.RenewToken(UserId);
            return Ok(tokenObj);
        }

    }
}
