using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using RestApiPractice.DataLayer.Models;
using Google.Apis.Auth;
using System.Net.Http;
using System.Text.Json;

namespace RestApiPractice.LogicLayer
{
    public class SpotifyLoginLogic
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public SpotifyLoginLogic(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task<SpotifyTokenResponse> LoginAsync(string code)
        {
            var clientId = _configuration["SpotifyConfig:ClientId"];
            var clientSecret = _configuration["SpotifyConfig:ClientSecret"];
            var redirectUri = _configuration["SpotifyConfig:RedirectUri"];
            // Console.WriteLine($"[SpotifyLoginLogic] ClientId: {clientId}");
            // Console.WriteLine($"[SpotifyLoginLogic] ClientId: {clientSecret}");
            // Console.WriteLine($"[SpotifyLoginLogic] ClientId: {redirectUri}");
            var formData = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", redirectUri! },
                { "client_id", clientId! },
                { "client_secret", clientSecret! }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Spotify token exchange failed: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<SpotifyTokenResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return token!;
        }
    }

}