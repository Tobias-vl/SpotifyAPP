namespace Spotify_backend
{
    public class AppSettings
    {
      public Appconfig app { get; set; }
    }

    public class Appconfig
    {
        public string clientId { get; set; }
        public string clientSecret { get; set; }
    }
}
