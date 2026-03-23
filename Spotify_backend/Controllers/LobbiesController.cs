using Microsoft.AspNetCore.Mvc;
using Spotify_backend.Models;
using Spotify_backend.Services;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Spotify_backend.Hubs;

using Microsoft.AspNetCore.SignalR;

namespace Spotify_backend.Controllers
{
    public class LobbiesController : ControllerBase
    {
        private readonly SpotifyPlayerManager _playerManager;
        private readonly SpotifyPlaylistService _spotifyPlaylistService;
        private readonly SpotifyGetInfo _spotifyGetInfo;
        private readonly MediaPlayer _mediaPlayer;
        private readonly LobbyManager _lobbies;
        private readonly IHubContext<LobbyHub> _hubContext;

        public LobbiesController(
            SpotifyPlayerManager playerManager,
            SpotifyPlaylistService spotifyPlaylistService,
            SpotifyGetInfo spotifyGetInfo,
            MediaPlayer mediaPlayer,
            LobbyManager Lobbies,
            IHubContext<LobbyHub> hubContext)
        {
            _playerManager = playerManager;
            _spotifyPlaylistService = spotifyPlaylistService;
            _spotifyGetInfo = spotifyGetInfo;
            _mediaPlayer = mediaPlayer;
            _lobbies = Lobbies;
            _hubContext = hubContext;
        }

        [HttpPost("create")]
        public Lobby CreateLobby(string hostUserId, string lobbyName)
        {
            Lobby lobby = _lobbies.CreateLobby(hostUserId, lobbyName);
            return lobby;
        }


        [HttpPost("{lobbyId}/join")]
        public async Task<IActionResult> JoinLobby(string lobbyId, string userId)
        {
            bool status = _lobbies.JoinLobby(lobbyId, userId);

            if (!status)
            {
                throw new Exception("The Lobby you are trying to join could not be found");
            }

            await _hubContext.Clients.Group(lobbyId).SendAsync("MemberJoined", userId);
            return Ok();
        }

        [HttpPost("{lobbyId}/leave")]
        public async Task<IActionResult> LeaveLobby(string lobbyId, string userId)
        {
            bool status = _lobbies.LeaveLobby(lobbyId, userId);

            if (!status)
            {
                throw new Exception("User not found in lobby");
            }

            await _hubContext.Clients.Group(lobbyId).SendAsync("MemberLeft", userId);

            return Ok();
        }


        [HttpGet("list")]
        public List<Lobby> ListLobbies()
        {
            return _lobbies.ListLobbies();
        }


        [HttpGet("{lobbyId}")]
        public Lobby GetLobby(string lobbyId)
        {
            return _lobbies.GetLobby(lobbyId);
        }

    }

}
