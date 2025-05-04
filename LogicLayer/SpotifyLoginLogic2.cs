using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.Json;
using RestApiPractice.DataLayer.Models;
using RestApiPractice.Providers;

namespace RestApiPractice.LogicLayer
{
    public class SpotifyLoginLogic
    {
        private readonly SpotifyConfigOptions _config;
        private readonly HttpClient _httpClient;

        public SpotifyLoginLogic(IOptions<SpotifyConfigOptions> config, HttpClient httpClient)
        {
            _config = config.Value;
            _httpClient = httpClient;
        }

        public async Task<SpotifyTokenResponse> LoginAsync(string code)
        {
            var formData = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", _config.RedirectUri },
                { "client_id", _config.ClientId },
                { "client_secret", _config.ClientSecret }
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