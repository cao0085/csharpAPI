

namespace RestApiPractice.DataLayer.Models
{


    public class SpotifyLoginRequest
    {
        public string? Code { get; set; }
    }


    public class SpotifyTokenResponse
    {
        public string? access_token { get; set; }

        public string? refresh_token { get; set; }

        public int? expires_in { get; set; }

        public string? scope { get; set; }

        public string? token_type { get; set; }
    }

}


