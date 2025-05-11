using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

using RestApiPractice.DataLayer.Models;
using RestApiPractice.Providers;
using RestApiPractice.Extensions;
using RestApiPractice.Repositories;


namespace RestApiPractice.LogicLayer
{
    public class SpotifyApiLogic
    {
        private readonly HttpClient _http;
        private readonly SpotifyRepository _repo;
        private readonly IUserContextService _userContext;

        public SpotifyApiLogic(HttpClient http, SpotifyRepository repo, IUserContextService userContext)
        {
            _http = http;
            _repo = repo;
            _userContext = userContext;
            _http.BaseAddress = new Uri("https://api.spotify.com");
        }


        public async Task<bool> TransferPlaybackAsync(string deviceId, bool play)
        {
            var uid = _userContext.Uid;
            if (string.IsNullOrEmpty(uid)) return false;

            var token = await _repo.GetAccessTokenAsync(uid);
            if (string.IsNullOrEmpty(token)) return false;

            var request = new HttpRequestMessage(HttpMethod.Put, "/v1/me/player");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var body = new
            {
                device_ids = new[] { deviceId },
                play = play
            };

            request.Content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );
            var response = await _http.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    Console.Error.WriteLine($"Spotify transfer error: {response.StatusCode}, body: {errorBody}");
                }

            return response.IsSuccessStatusCode;
        }


        public async Task<bool> LoadTrack(string deviceId, string trackUri)
        {
            string? token = await GetUserSpotifyTokenAsync();
            if (token == null) return false;

            var url = $"v1/me/player/play?device_id={deviceId}";
            var request = new HttpRequestMessage(HttpMethod.Put,url);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Console.WriteLine($"Loading track {trackUri} on device {deviceId}");
            var body = new
            {
                uris = new[] { $"spotify:track:{trackUri}" },
                position_ms = 0
            };

            request.Content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );
            var response = await _http.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    Console.Error.WriteLine($"Spotify transfer error: {response.StatusCode}, body: {errorBody}");
                }

            return response.IsSuccessStatusCode;
        }

        public async Task<string?> GetUserSpotifyTokenAsync()
        {
            var uid = _userContext.Uid;
            if (string.IsNullOrEmpty(uid)) return null;

            var token = await _repo.GetAccessTokenAsync(uid);
            return string.IsNullOrEmpty(token) ? null : token;
        }

        public async Task<bool> ClearSpotifyTokensAsync()
        {
            var uid = _userContext.Uid;
            if (string.IsNullOrEmpty(uid)) return false;

            var result = await _repo.ClearSpotifyTokensAsync(uid);
            return result;
        }
    }
}