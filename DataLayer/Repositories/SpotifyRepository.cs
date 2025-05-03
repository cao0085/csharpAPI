using RestApiPractice.Providers;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using RestApiPractice.DataLayer.Models;
using RestApiPractice.DataLayer.Models.GoogleModel;



namespace RestApiPractice.Repositories
{
    public class SpotifyRepository
    {
        private readonly FirestoreDb _db;

        public SpotifyRepository(IFirebaseProvider provider)
        {
            _db = provider.GetDb();
        }

        // public async Task<SpotifyTokenResponse> LoginAsync(string uid, string code)
        // {
        //     // 1. 讀取設定
        //     var clientId = _configuration["Spotify:ClientId"];
        //     var clientSecret = _configuration["Spotify:ClientSecret"];
        //     var redirectUri = _configuration["Spotify:RedirectUri"];

        //     // 2. 用 code 換 token
        //     var httpClient = new HttpClient();
        //     var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
        //     request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        //     {
        //         {"grant_type", "authorization_code"},
        //         {"code", code},
        //         {"redirect_uri", redirectUri!},
        //         {"client_id", clientId!},
        //         {"client_secret", clientSecret!}
        //     });

        //     var response = await httpClient.SendAsync(request);
        //     response.EnsureSuccessStatusCode();

        //     var json = await response.Content.ReadAsStringAsync();
        //     var token = JsonSerializer.Deserialize<SpotifyTokenResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        //     // 3. 儲存到 Firebase（可再抽出去）
        //     await SaveTokenToFirebaseAsync(uid, token);

        //     return token!;
        // }

    }
}