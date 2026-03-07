using Microsoft.AspNetCore.Mvc;
using Spotify_backend.Models;
using Spotify_backend.Services;
using System.Reflection.Metadata.Ecma335;

namespace Spotify_backend.Controllers
{
    public class TestController : ControllerBase
    {
        private readonly SpotifyPlayerManager _playerManager;
        private readonly SpotifyPlaylistService _spotifyPlaylistService;
        private readonly SpotifyGetInfo _spotifyGetInfo;
        private readonly MediaPlayer _mediaPlayer;

        public TestController(
            SpotifyPlayerManager playerManager,
            SpotifyPlaylistService spotifyPlaylistService,
            SpotifyGetInfo spotifyGetInfo,
            MediaPlayer mediaPlayer)
        {
            _playerManager = playerManager;
            _spotifyPlaylistService = spotifyPlaylistService;
            _spotifyGetInfo = spotifyGetInfo;
            _mediaPlayer = mediaPlayer;
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

        [HttpPost("Pause/{userId}")]
        public async Task<IActionResult> Pause(string userId)
        {
            var player = _playerManager.Get(userId);

            if (player == null)
                return NotFound("Player not found in manager.");

            var devices = await _mediaPlayer.Getdevice(player.AccessToken);

            string device_id = GetDeviceID(devices);

            await _mediaPlayer.Pause(device_id,  player.AccessToken);

            return Ok();
        }

        [HttpPost("Skip/{userId}")]
        public async Task<IActionResult> Skip(string userId)
        {
            var player = _playerManager.Get(userId);

            if (player == null)
                return NotFound("Player not found in manager.");

            await _mediaPlayer.Skip(player.AccessToken);

            return Ok();
        }

        [HttpPost("Resume/{userId}")]
        public async Task<IActionResult> Resume(string userId)
        {
            var player = _playerManager.Get(userId);

            if (player == null)
                return NotFound("Player not found in manager.");

            var devices = await _mediaPlayer.Getdevice(player.AccessToken);

            string device_id = GetDeviceID(devices);

            await _mediaPlayer.Resume(device_id, player.AccessToken);

            return Ok();
        }

        [HttpPost("Repeat/{userId}")]
        public async Task<IActionResult> Loop(string userId)
        {
            var player = _playerManager.Get(userId);

            if (player == null)
                return NotFound("Player not found in manager.");

            var devices = await _mediaPlayer.Getdevice(player.AccessToken);

            string device_id = GetDeviceID(devices);

            await _mediaPlayer.Repeat(player.AccessToken);

            return Ok();
        }

        [HttpGet("Devices/{userId}")]
        public async Task<IActionResult> GetDevice(string userId)
        {
            var player = _playerManager.Get(userId);

            if (player == null)
                return BadRequest("Player not found in manager.");

            var json = await _mediaPlayer.Getdevice(player.AccessToken);

            return Ok(json);
        }

        public string GetDeviceID(Device devices)
        {
            string device_id = "";
            if (devices == null)
                return "Not found";

            foreach (var device in devices.device)
            {
                if (device.name == "TOBIAS-GALAXAY-")
                {
                    device_id = device.id;
                }
            }
            return device_id;

        }




    }

}
