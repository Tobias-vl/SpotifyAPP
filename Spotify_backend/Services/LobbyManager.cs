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
        private readonly Dictionary<string, Lobby> _lobbies = new();

        public Lobby CreateLobby(string hostUserId, string lobbyName)
        {
                Lobby lobby = new()
            {
                HostUserId = hostUserId,
                LobbyName = lobbyName,
            };
            lobby.MembersUserId.Add(hostUserId);
            _lobbies[lobby.LobbyId] = lobby;
            return lobby;
        }

        public bool JoinLobby(string lobbyId, string userId)
        {
            if (_lobbies.TryGetValue(lobbyId, out var lobby))
            {
                if (!lobby.MembersUserId.Contains(userId))
                {
                    lobby.MembersUserId.Add(userId);
                }
                return true;
            }
            return false;
        }

        public bool LeaveLobby(string lobbyId, string userId)
        {
            if (_lobbies.TryGetValue(lobbyId, out var lobby))
            {
                lobby.MembersUserId.Remove(userId);
                return true;
            }
            return false;
        }

        public Lobby? GetLobby(string lobbyId)
        {
            _lobbies.TryGetValue(lobbyId, out var lobby);
            return lobby;
        }

        public List<Lobby> ListLobbies()
        {
            return _lobbies.Values.ToList();
        }
    }
}
