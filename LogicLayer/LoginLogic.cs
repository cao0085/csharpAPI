using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using RestApiPractice.DataLayer.Models.GoogleModel;
using Google.Apis.Auth;

namespace RestApiPractice.LogicLayer
{
    public class GoogleLoginLogic
    {
        private readonly IConfiguration _configuration;

        public GoogleLoginLogic(IConfiguration configuration)
        {
            _configuration = configuration;
        }
                
        public async Task<GoogleLoginResponse> LoginAsync(GoogleLoginRequest req)
        {   
            var payload = await GoogleJsonWebSignature.ValidateAsync(req.IdToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _configuration["GoogleLogicAuth:ClientId"] }
            });

            return new GoogleLoginResponse
            {
                Email = payload.Email,
                EmailVerified = payload.EmailVerified,
                Name = payload.Name,
                GivenName = payload.GivenName,
                FamilyName = payload.FamilyName,
                Picture = payload.Picture,
                Locale = payload.Locale,
                GoogleUserId = payload.Subject
            };
        }
    }

    public class FakeGoogleLogin
    {
        public Task<GoogleLoginResponse> LoginAsync(GoogleLoginRequest req)
        {
            // 模擬回傳假的登入結果（不使用 req.IdToken）
            return Task.FromResult(new GoogleLoginResponse
            {
                Email = "testuser@example.com",
                Name = "Fake User",
                Picture = "https://example.com/avatar.jpg",
                GoogleUserId = "fake-google-user-id-001"
            });
        }
    }
}