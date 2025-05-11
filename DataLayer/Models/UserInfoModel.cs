using Google.Cloud.Firestore;

namespace RestApiPractice.DataLayer.Models
{   


    [FirestoreData]
    public class UserInfoEntity
    {
        [FirestoreProperty] public string Uid { get; set; } = string.Empty;
        [FirestoreProperty] public string Email { get; set; } = string.Empty;
        [FirestoreProperty] public long? Role { get; set; }
        [FirestoreProperty] public long? RegisterSource { get; set; }
        [FirestoreProperty] public string Name { get; set; } = string.Empty;
        [FirestoreProperty] public string ShortName { get; set; } = string.Empty;
        [FirestoreProperty] public string Picture { get; set; } = string.Empty;
        [FirestoreProperty] public string GoogleUserId { get; set; } = string.Empty;
        [FirestoreProperty] public Timestamp CreatedAt { get; set; }
        [FirestoreProperty] public Timestamp LastLoginAt { get; set; }
    }

    [FirestoreData]
    public class BasicUserInfo
    {
        [FirestoreProperty] public string Uid { get; set; } = string.Empty;
        [FirestoreProperty] public string Email { get; set; } = string.Empty;
        [FirestoreProperty] public long? Role { get; set; }
        [FirestoreProperty] public long? RegisterSource { get; set; }
        [FirestoreProperty] public string Name { get; set; } = string.Empty;
        [FirestoreProperty] public string ShortName { get; set; } = string.Empty;
        [FirestoreProperty] public string Picture { get; set; } = string.Empty;
        [FirestoreProperty] public string GoogleUserId { get; set; } = string.Empty;
        [FirestoreProperty] public Timestamp CreatedAt { get; set; }
    }

    public class UserInfoDto
    {   
        public string Uid { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public long? Role { get; set; }
        public long? RegisterSource { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;
        public string GoogleUserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}