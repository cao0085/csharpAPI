using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;

public class GoogleLoginRequest
{
    public string IdToken { get; set; } = string.Empty;
}

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("google")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest req)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(req.IdToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { "821577494316-kjups39juc8v5dvfograkau0fod1efsu.apps.googleusercontent.com" }
            });

            // payload.Email、payload.Name、payload.Picture、payload.Sub（唯一ID）
            return Ok(new
            {
                email = payload.Email,
                name = payload.Name,
                picture = payload.Picture,
                googleUserId = payload.Subject
            });
        }
        catch (InvalidJwtException ex)
        {
            return Unauthorized(new { error = "ID token 驗證失敗", message = ex.Message });
        }
    }
}