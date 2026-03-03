using Microsoft.AspNetCore.Mvc;
using Spotify_backend.Services;
using System.Reflection.Metadata.Ecma335;

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

        [HttpGet("playlist/{userId}")]
        public async Task<IActionResult> TestGetPlaylists(string userId)
        {
            var player = _playerManager.Get(userId);
            
            if (player == null)
                return BadRequest("Player not found in manager.");

            var playlist = await _spotifyPlaylistService.GetPlaylists(player.AccessToken, userId);

            return Ok(playlist);
        }

        [HttpGet("playlistItems/{userId}")]
        public async Task<IActionResult> TestGetPlaylistItems(string PlaylistId, string userId)
        {
            var player = _playerManager.Get(userId);

            if (player == null)
                return BadRequest("Player not found in manager.");

            var playlistItems = await _spotifyPlaylistService.GetPlaylistItems(player.AccessToken, PlaylistId);

            return Ok(playlistItems);
        }

        [HttpGet("CurrentTrack/{userId}")]
        public async Task<IActionResult> GetCurrentTrack(string userId)
        {
            var player = _playerManager.Get(userId);

            if (player == null)
                return BadRequest("Player not found in manager.");

            var CurrentTrack = await _spotifyPlaylistService.GetCurrentTrack(player.AccessToken);

            return Ok(CurrentTrack);
        }

        [HttpGet("GetTracks/{userId}")]
        public async Task<IActionResult> GetTracks(string userId)
        {
            var player = _playerManager.Get(userId);

            if (player == null)
                return BadRequest("Player not found in manager.");

            var json = await _spotifyPlaylistService.GetTracks(player.AccessToken, userId);

            return Ok(json);
        }



    }

}
