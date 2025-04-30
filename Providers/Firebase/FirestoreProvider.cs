using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Options;
using RestApiPractice.Settings;


namespace RestApiPractice.Providers.Firebase
{
    /// <summary>
    /// FirestoreProvider class
    /// </summary>
    public class FirestoreProvider : IFirestoreProvider
    {
        private readonly FirestoreDb _firestoreDb;

        public FirestoreProvider(IOptions<FirebaseConfigOptions> config)
        {   
            var firebaseOptions = config.Value;
            

            if (string.IsNullOrEmpty(firebaseOptions.ServiceAccountKeyPath) || string.IsNullOrEmpty(firebaseOptions.ProjectId))
            {   
                throw new InvalidOperationException("Can't get FirebaseConfig , Check appsettings.json");
            };
            
            GoogleCredential credential = GoogleCredential.FromFile(firebaseOptions.ServiceAccountKeyPath);

            _firestoreDb = new FirestoreDbBuilder
            {
                ProjectId = firebaseOptions.ProjectId,
                Credential = credential
            }.Build();

        }

        public FirestoreDb GetFirestoreDb()
        {
            return _firestoreDb;
        }
    }

}

