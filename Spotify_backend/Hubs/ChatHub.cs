using Microsoft.AspNetCore.SignalR;

namespace Spotify_backend.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", "test");
    }
}
