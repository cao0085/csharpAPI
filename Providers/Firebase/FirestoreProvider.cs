using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Options;
using RestApiPractice.Settings;
using System.Text.Json;


namespace RestApiPractice.Providers
{   

    public interface IFirebaseProvider
    {
        FirestoreDb GetDb();
    }

    public class FirestoreProvider : IFirebaseProvider
    {
        private readonly FirestoreDb _db;

        public FirestoreProvider(IOptions<FirebaseConfigOptions> config)
        {   
            var firebaseOptions = config.Value;
            

            if (string.IsNullOrEmpty(firebaseOptions.ProjectId) || string.IsNullOrEmpty(firebaseOptions.PrivateKey) || string.IsNullOrEmpty(firebaseOptions.ClientEmail))
            {   
                throw new InvalidOperationException("Can't get FirebaseConfig , Check appsettings.json");
            };

            var credentialJson = JsonSerializer.Serialize(new
            {
                type = "service_account",
                project_id = firebaseOptions.ProjectId,
                private_key = firebaseOptions.PrivateKey.Replace("\\n", "\n"),
                client_email = firebaseOptions.ClientEmail,
                token_uri = "https://oauth2.googleapis.com/token"
            });

            GoogleCredential credential = GoogleCredential.FromFile(credentialJson);

            _db = new FirestoreDbBuilder
            {
                ProjectId = firebaseOptions.ProjectId,
                Credential = credential
            }.Build();

        }

        public FirestoreDb GetDb() => _db;
    }

    public class FakeFirestoreProvider : IFirebaseProvider
    {
        public FirestoreDb GetDb()
        {
            // 回傳 null 或模擬的物件
            throw new NotImplementedException("這是測試用，不實作真實 Firebase");
        }
    }
}

