using Microsoft.AspNetCore.Mvc;
using Spotify_backend.Models;
using Spotify_backend.Services;
using System.Reflection.Metadata.Ecma335;

namespace Spotify_backend.Controllers
{
    public class LobbiesController : ControllerBase
    {
        private readonly SpotifyPlayerManager _playerManager;
        private readonly SpotifyPlaylistService _spotifyPlaylistService;
        private readonly SpotifyGetInfo _spotifyGetInfo;
        private readonly MediaPlayer _mediaPlayer;

        public LobbiesController(
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

        [HttpPost("create")]
        public IActionResult CreateLobby(string hostUserId, string lobbyName)
        {
            return Ok();
        }


        [HttpPost("{lobbyId}/join")]
        public IActionResult JoinLobby(string lobbyId, string userId)
        {
            return Ok();
        }


        [HttpGet("list")]
        public IActionResult ListLobbies()
        {
            return Ok();
        }


        [HttpGet("{lobbyId}")]
        public IActionResult GetLobby(string lobbyId)
        {
            return Ok();
        }

    }

}
