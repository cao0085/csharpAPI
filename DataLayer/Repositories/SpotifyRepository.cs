using RestApiPractice.Providers;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using RestApiPractice.DataLayer.Models;
using RestApiPractice.DataLayer.Models.GoogleModel;

using RestApiPractice.Extensions;



namespace RestApiPractice.Repositories
{
    public class SpotifyRepository
    {
        private readonly FirestoreDb _db;

        public SpotifyRepository(IFirebaseProvider provider)
        {
            _db = provider.GetDb();
        }

        public async Task<string?> GetAccessTokenAsync(string uid)
        {
            var doc = await _db.Collection("spotifyToken").Document(uid).GetSnapshotAsync();
            if (doc.Exists && doc.TryGetValue("access_token", out string token))
            {
                return token;
            }

            return null;
        }

        public async Task<bool> ClearSpotifyTokensAsync(string uid)
        {
            var docRef = _db.Collection("spotifyToken").Document(uid);
            var snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                await docRef.DeleteAsync();
                return true;
            }

            return false;
        }

    }
}