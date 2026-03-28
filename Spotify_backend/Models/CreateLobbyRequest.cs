namespace Spotify_backend.Models
{
    public class CreateLobbyRequest
    {
        public string UserId { get; set; }
        public string LobbyName { get; set; }
    }

    public class JoinRequest { public string UserId { get; set; } }
    public class LeaveRequest { public string UserId { get; set; } }
}
