using Google.Cloud.Firestore;

namespace RestApiPractice.DataLayer.Models.GoogleModel
{

    public class GoogleLoginRequest
    {   
        public string? IdToken { get; set; }
    }

    public class GoogleLoginResponse
    {
        public string Email { get; set; } = string.Empty;
        public bool EmailVerified { get; set; } = false;
        public string Name { get; set; } = string.Empty;
        public string GivenName { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;
        public string Locale { get; set; } = string.Empty;
        public string GoogleUserId { get; set; } = string.Empty;
    }

    public class GoogleAuthInfoEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;
        public string GoogleUserId { get; set; } = string.Empty;
        public Timestamp CreatedAt { get; set; }
    }
    
}