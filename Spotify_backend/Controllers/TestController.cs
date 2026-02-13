using Microsoft.AspNetCore.Mvc;
using Spotify_backend.Models;
using Spotify_backend.Services;
using System.Threading.Tasks;

namespace Spotify_backend.Controllers
{
    public class TestController : ControllerBase
    {
        private readonly SpotifyPlayerManager _playerManager;
        private readonly SpotifyPlaylistService _spotifyPlaylistService;
        private readonly SpotifyGetInfo _spotifyGetInfo;

        public TestController(
            SpotifyPlayerManager playerManager,
            SpotifyPlaylistService spotifyPlaylistService,
            SpotifyGetInfo spotifyGetInfo)
        {
            _playerManager = playerManager;
            _spotifyPlaylistService = spotifyPlaylistService;
            _spotifyGetInfo = spotifyGetInfo;
        }

        [HttpGet("profile/{userId}")]
        public async Task<IActionResult> TestProfile(string userId)
        {
            var player = _playerManager.Get(userId);

            if (player == null)
                return BadRequest("Player not found in manager.");


            var profile = await _spotifyGetInfo.GetProfile(player.AccessToken, userId);

            return Ok(profile);
        }

        [HttpGet("playlist/{userId}")]
        public async Task<IActionResult> TestGetPlaylists(string userId)
        {
            var player = _playerManager.Get(userId);

            if (player == null)
                return BadRequest("Player not found in manager.");

            var playlist = await _spotifyPlaylistService.GetPlaylists(player.AccessToken, userId);

            return Ok(playlist);
        }

    }

}
