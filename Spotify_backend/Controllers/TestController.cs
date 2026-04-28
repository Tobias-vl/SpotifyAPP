using Microsoft.AspNetCore.Components.Web;
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
        private readonly LobbyManager _LobbyManager;

        public TestController(
            SpotifyPlayerManager playerManager,
            SpotifyPlaylistService spotifyPlaylistService,
            SpotifyGetInfo spotifyGetInfo,
            MediaPlayer mediaPlayer,
            LobbyManager lobbyManager
            )
        {
            _playerManager = playerManager;
            _spotifyPlaylistService = spotifyPlaylistService;
            _spotifyGetInfo = spotifyGetInfo;
            _mediaPlayer = mediaPlayer;
            _LobbyManager = lobbyManager;
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

            await _mediaPlayer.Pause(device_id, player.AccessToken);

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

        [HttpGet("HasAllVoted/{LobbyId}")]
        public IActionResult HasAllVoted(string LobbyId)
        {
            var hasVoted = _LobbyManager.HasEveryPlayerVoted(LobbyId);
            return Ok(hasVoted);
        }

        [HttpPost("Voted/{LobbyId}/{userId}")]
        public IActionResult Voted(string LobbyId, string userId)
        {
            var Voted = _LobbyManager.Voted(LobbyId, userId);
            return Ok(Voted);
        }

        public string GetDeviceID(Device devices)
        {
            string device_id = "";
            if (devices == null)
                return "Not found";

            foreach (var device in devices.device)
            {
                if (device.name == "DESKTOP-HLBI1UD")
                {
                    device_id = device.id;
                }
            }
            return device_id;

        }





    }

}
