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
        private static readonly Dictionary<string, Lobby> dictionary = new();
        private readonly Dictionary<string, Lobby> _lobbies = dictionary;
        private readonly SpotifyPlayerManager _playermanger;

        public LobbyManager(Dictionary<string, Lobby> lobbies, SpotifyPlayerManager playermanger)
        {
            _lobbies = lobbies;
            _playermanger = playermanger;
        }

        public Task<Lobby> CreateLobby(string hostUserId, string lobbyName)
        {
            Lobby lobby = new()
            {
                HostUserId = hostUserId,
                LobbyName = lobbyName,
            };
            var player = _playermanger.Get(hostUserId);

            lobby.MembersUserId.Add(player.Name);
            _lobbies[lobby.LobbyId] = lobby;
            return Task.FromResult(lobby);
        }

        public bool JoinLobby(string lobbyId, string userId)
        {
            if (_lobbies.TryGetValue(lobbyId, out var lobby))
            {
                if (!lobby.MembersUserId.Contains(userId))
                {
                    var player = _playermanger.Get(userId);
                    lobby.MembersUserId.Add(player.Name);
                }
                return true;
            }
            return false;
        }

        public bool LeaveLobby(string lobbyId, string userId)
        {
            if (_lobbies.TryGetValue(lobbyId, out var lobby))
            {
                var player = _playermanger.Get(userId);
                lobby.MembersUserId.Remove(player.Name);
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
