
using RestApiPractice.DataLayer.Models;
using RestApiPractice.Repositories;
using RestApiPractice.DataLayer.Models.GoogleModel;
using Google.Cloud.Firestore;


namespace RestApiPractice.LogicLayer
{
    public class AccountLogic
    {   
        private readonly AccountRepository _repo;
        
        public AccountLogic(AccountRepository repo)
        {
            _repo = repo;
        }

        public async Task<UserInfoDto> GetUserInfoAsync(GoogleLoginResponse loginRes)
        {   

            bool isUserExist = await _repo.IsUserExistAsync(loginRes.Email);

            if (!isUserExist)
            {   
                await _repo.CreateBasicUserInfo(loginRes);
            }

            var userUID = await _repo.GetUserInfoUIDAsync(loginRes.Email);
            var userInfoEntity = await _repo.GetUserInfoAsync(userUID);
            
            return new UserInfoDto
            {   
                Uid = userInfoEntity.Uid,
                GoogleUserId = userInfoEntity.GoogleUserId,
                Name = userInfoEntity.Name,
                Email = userInfoEntity.Email,
                Picture = userInfoEntity.Picture,
                RegisterSource = userInfoEntity.RegisterSource,
                Role = userInfoEntity.Role,
                CreatedAt = userInfoEntity.CreatedAt.ToDateTime()
            };
        }

        public async Task<bool> SetSpotifyToken(string uid ,SpotifyTokenResponse tokenRes)
        {
            // Create DataFormat
            var token = new Dictionary<string, object?>
            {
                { "access_token", tokenRes.access_token },
                { "refresh_token", tokenRes.refresh_token },
                { "expires_in", tokenRes.expires_in },
                { "scope", tokenRes.scope },
                { "token_type", tokenRes.token_type },
                { "created_at", Timestamp.GetCurrentTimestamp() }
            };
        
            return await _repo.SetSpotifyToken(uid , token);
        }
    }
}