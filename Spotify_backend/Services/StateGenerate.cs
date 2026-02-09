using System.Security.Cryptography;

namespace Spotify_backend.Services
{
    public class StateGenerate
    {

        public string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);

            char[] result = new char[length];
            for (int i = 0; i < length; i++)
                result[i] = chars[bytes[i] % chars.Length];

            return new string(result);
        }

        
    }
}
