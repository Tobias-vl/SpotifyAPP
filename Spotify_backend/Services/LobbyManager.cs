namespace Spotify_backend.Services
{
    public class Lobby
    {
        public string LobbyId { get; set; } = Guid.NewGuid().ToString();
        public string LobbyName { get; set; }
        public string HostUserId { get; set; }
        public List<string> MembersUserId { get; set; } = new();
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }

    public class LobbyManager
    {
        private readonly Dictionary<string, Lobby> _Lobbies = new();

        public Lobby CreateLobby(string hostUserId, string lobbyName)
        {
            Lobby lobby = new();
            lobby.HostUserId = hostUserId;
            lobby.LobbyName = lobbyName;
            _Lobbies[lobby.LobbyId] = lobby;
            return lobby;
        }
        public bool JoinLobby(string lobbyId, string userId)
        {
            _Lobbies[lobbyId].MembersUserId.Add(userId);
            return true;
        }
        public bool LeaveLobby(string lobbyId, string userId)
        {
            _Lobbies[lobbyId].MembersUserId.Remove(userId);
            return true;
        }
        public Lobby? GetLobby(string lobbyId)
        {
            return _Lobbies[lobbyId];
        }
        public List<Lobby> ListLobbies()
        {
            return _Lobbies.Values.ToList();
        }
    }
}
