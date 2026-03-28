using Microsoft.AspNetCore.SignalR;
using Spotify_backend.Services;

namespace Spotify_backend.Hubs;

public class LobbyHub : Hub
{
    private readonly LobbyManager _lobbyManager;
    private readonly SpotifyPlayerManager _playerManager;

    public LobbyHub(LobbyManager lobbyManager, SpotifyPlayerManager playerManager)
    {
        _lobbyManager = lobbyManager;
        _playerManager = playerManager;
    }

    public async Task JoinLobbyGroup(string lobbyId, string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId);
    }

    public async Task LeaveLobbyGroup(string lobbyId, string userId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyId);
    }
}
