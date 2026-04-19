using System.Reflection.Metadata.Ecma335;

namespace Spotify_backend.Services
{

    
    public class Lobby
    {
        public string LobbyId { get; set; } = string.Empty;
        public string LobbyName { get; set; } = string.Empty;
        public string HostUserId { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public List<Player> MembersUserId { get; set; } = new();
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }

    public class Player
    {
        public string Name { get; set; } = string.Empty;
        public bool Voted { get; set; } = false;
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
            var player = _playermanger.Get(hostUserId);

            Lobby lobby = new()
            {
                LobbyId = GenerateLobbyId(),
                HostUserId = hostUserId,
                HostName = player?.Name ?? hostUserId,
                LobbyName = lobbyName,
            };

            lobby.MembersUserId.Add(new Player { Name = player?.Name ?? hostUserId });
            _lobbies[lobby.LobbyId] = lobby;
            return Task.FromResult(lobby);
        }

        public bool JoinLobby(string lobbyId, string userId)
        {
            if (_lobbies.TryGetValue(lobbyId, out var lobby))
            {
                var player = _playermanger.Get(userId);
                var playerName = player?.Name ?? userId;

                if (!lobby.MembersUserId.Any(p => p.Name == playerName))
                {
                    lobby.MembersUserId.Add(new Player { Name = playerName });
                }
                return true;
            }
            return false;
        }

        public bool LeaveLobby(string lobbyId, string userId)
        {
            if (_lobbies.TryGetValue(lobbyId, out var lobby))
            {
                var playerName = _playermanger.Get(userId)?.Name ?? userId;
                var playerToRemove = lobby.MembersUserId.FirstOrDefault(p => p.Name == playerName);
                if (playerToRemove != null)
                {
                    lobby.MembersUserId.Remove(playerToRemove);
                }
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

        private string GenerateLobbyId(int length = 6)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string id;

            do
            {
                char[] buffer = new char[length];
                for (int i = 0; i < length; i++)
                {
                    int index = System.Security.Cryptography.RandomNumberGenerator.GetInt32(letters.Length);
                    buffer[i] = letters[index];
                }
                id = new string(buffer);
            } while (_lobbies.ContainsKey(id));

            return id;
        }

        public bool HasEveryPlayerVoted(string lobbyID)
        {
            Lobby? lobby = GetLobby(lobbyID);
            
            if (lobby == null)
                return false;

            return lobby.MembersUserId.All(p => p.Voted);
        }

        public bool Voted(string lobbyId, string userId)
        {
            if (_lobbies.TryGetValue(lobbyId, out var lobby))
            {
                var playerName = _playermanger.Get(userId)?.Name ?? userId;
                var player = lobby.MembersUserId.FirstOrDefault(p => p.Name == playerName);
                if (player != null)
                {
                    player.Voted = true;
                    return true;
                }
            }
            return false;
        }
    }
}
