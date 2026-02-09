using Microsoft.AspNetCore.Mvc;
using System.Web;

using Spotify_backend.Services;

namespace Spotify_backend.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly ISpotifyAuthService _spotify;

        public AuthController(ISpotifyAuthService spotify)
        {
            _spotify = spotify;
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
            return Ok(tokenObj);
        }

    }
}
