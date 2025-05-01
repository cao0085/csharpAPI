using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Options;
using RestApiPractice.Settings;


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
            

            if (string.IsNullOrEmpty(firebaseOptions.ServiceAccountKeyPath) || string.IsNullOrEmpty(firebaseOptions.ProjectId))
            {   
                throw new InvalidOperationException("Can't get FirebaseConfig , Check appsettings.json");
            };
            
            GoogleCredential credential = GoogleCredential.FromFile(firebaseOptions.ServiceAccountKeyPath);

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

