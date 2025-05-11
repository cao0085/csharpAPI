using System.Security.Claims;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using RestApiPractice.DataLayer.Models;
using RestApiPractice.Providers;


namespace RestApiPractice.Extensions
{   
    public interface IUserContextService
    {
        string? Uid { get; }
    }

    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFirebaseProvider _firebaseProvider;

        public UserContextService(IHttpContextAccessor accessor, IFirebaseProvider firebaseProvider)
        {
            _httpContextAccessor = accessor;
            _firebaseProvider = firebaseProvider;
        }

        public string? Uid =>
            _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public async Task<UserInfoEntity?> GetCurrentUserAsync()
        {
            if (string.IsNullOrEmpty(Uid)) return null;

            var db = _firebaseProvider.GetDb();
            var snapshot = await db.Collection("users").Document(Uid).GetSnapshotAsync();
            return snapshot.Exists ? snapshot.ConvertTo<UserInfoEntity>() : null;
        }
    }

}