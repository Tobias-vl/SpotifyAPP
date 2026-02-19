using Spotify_backend.Models;

namespace Spotify_backend.Services
{
    public class SpotifyPlayerManager 
    {
        public readonly Dictionary<string, SpotifyPlayer> _player = new();

        public void AddOrUpdate(string key,  SpotifyPlayer player)
        {
            _player[key] = player;
        }

        public SpotifyPlayer? Get(string key)
        {
            _player.TryGetValue(key, out var player);
            return player;
        }

        public void ReplaceKey(string oldKey, string newKey)
        {
            if (_player.TryGetValue(oldKey, out var player))
            {
                _player.Remove(oldKey);
                _player[newKey] = player;
            }
        }

        public bool Remove(string key)
        {
            return _player.Remove(key);
        }


            
    }
}
