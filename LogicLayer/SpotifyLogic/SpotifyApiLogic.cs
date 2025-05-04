using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.Json;
using RestApiPractice.DataLayer.Models;
using RestApiPractice.Providers;
using RestApiPractice.Extensions;
using RestApiPractice.Repositories;
using System.Net.Http.Headers;

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
            _http.BaseAddress = new Uri("https://api.spotify.com/");
        }

        public async Task<string?> GetSpotifyEmailAsync()
        {
            var uid = _userContext.Uid;
            if (string.IsNullOrEmpty(uid)) return null;

            var token = await _repo.GetAccessTokenAsync(uid);
            if (string.IsNullOrEmpty(token)) return null;

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var res = await _http.GetAsync("https://api.spotify.com/v1/me");
            if (!res.IsSuccessStatusCode) return null;

            var json = JsonDocument.Parse(await res.Content.ReadAsStringAsync()).RootElement;
            return json.GetProperty("email").GetString();
        }
    }
}