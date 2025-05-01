using RestApiPractice.Providers;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using RestApiPractice.DataLayer.Models;
using RestApiPractice.DataLayer.Models.GoogleModel;



namespace RestApiPractice.Repositories
{
    public class AccountRepository
    {
        private readonly FirestoreDb _db;

        public AccountRepository(IFirebaseProvider provider)
        {
            _db = provider.GetDb();
        }

        public async Task<bool> IsUserExistAsync(string email)
        {
            var emailDocRef = _db.Collection("userEmailMap").Document(email);
            var emailSnapshot = await emailDocRef.GetSnapshotAsync();

            return emailSnapshot.Exists;
        }

        public async Task<bool> CreateBasicUserInfo(GoogleLoginResponse loginRes)
        {
            var userEmailMapRef = _db.Collection("userEmailMap").Document(loginRes.Email);
            var snapshot = await userEmailMapRef.GetSnapshotAsync();
            if (snapshot.Exists)
                return false;
            
            // Create a new user UID and store it in the userEmailMap collection
            var uid = Guid.NewGuid().ToString();
            var basicUserInfo = new BasicUserInfo
            {   
                Uid = uid,
                Name = loginRes.Name,
                ShortName = loginRes.Name,
                Email = loginRes.Email,
                Picture = loginRes.Picture,
                Role = 33,
                RegisterSource = 2,
                GoogleUserId = loginRes.GoogleUserId,
                CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow)
            };

            var batch = _db.StartBatch();

            batch.Set(userEmailMapRef, new { UID = uid });
            batch.Set(_db.Collection("users").Document(uid), basicUserInfo);

            await batch.CommitAsync();

            return true;
        }

        // GetInfo

        public async Task<string> GetUserInfoUIDAsync(string email)
        {
            var emailDocRef = _db.Collection("userEmailMap").Document(email);
            var emailSnapshot = await emailDocRef.GetSnapshotAsync();

            return emailSnapshot.GetValue<string>("UID");
        }

        public async Task<UserInfoEntity> GetUserInfoAsync(string userUID)
        {
            var userDocRef = _db.Collection("users").Document(userUID);
            var userDoc = await userDocRef.GetSnapshotAsync();

            return userDoc.ConvertTo<UserInfoEntity>();
        }


    }
}