using Microsoft.AspNetCore.SignalR;
using Spotify_backend.Services;

namespace Spotify_backend.Hubs;

public class LobbyHub : Hub
{
    private readonly LobbyManager? _lobbyManager;
    
    // Group users by lobby
    public async Task JoinLobby(string lobbyId, string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId);
        var lobby = _lobbyManager.GetLobby(lobbyId);
        await Clients.Group(lobbyId).SendAsync("MemberJoined", userId);
    }
    
    public async Task LeaveLobby(string lobbyId, string userId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyId);
        var lobby = _lobbyManager.GetLobby(lobbyId);
        await Clients.Group(lobbyId).SendAsync("MemberLeft", userId);
    }
    
    public async Task SendMessage(string lobbyId, string userId, string message)
    {
        await Clients.Group(lobbyId).SendAsync("ReceiveMessage", new { userId, message });
    }
}
