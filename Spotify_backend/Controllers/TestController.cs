using Microsoft.AspNetCore.Mvc;
using Spotify_backend.Models;
using Spotify_backend.Services;

namespace Spotify_backend.Controllers
{
    public class TestController : ControllerBase
    {
        private readonly SpotifyPlayerManager _playerManager;
        private readonly SpotifyGetInfo _spotifyGetInfo;

        public TestController(
            SpotifyPlayerManager playerManager,
            SpotifyGetInfo spotifyGetInfo)
        {
            _playerManager = playerManager;
            _spotifyGetInfo = spotifyGetInfo;
        }

        [HttpGet("profile/{userId}")]
        public async Task<IActionResult> TestProfile(string userId)
        {
            var player = _playerManager.Get(userId);

            if (player == null || player.AccessToken == null)
                return BadRequest("Player not found in manager. or AccessToken er null");


            var profile = await _spotifyGetInfo.GetProfile(player.AccessToken, userId);

            return Ok(profile);
        }

    }

}
