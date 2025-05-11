
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Google.Apis.Auth;

using RestApiPractice.Extensions;
using RestApiPractice.LogicLayer;
using RestApiPractice.DataLayer.Models;
using RestApiPractice.DataLayer.Models.GoogleModel;



[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{   
    private readonly JwtService _jwtService;

    public AuthController(JwtService jwtService)
    {
        _jwtService = jwtService;
    }

    [HttpPost("google")]
    public async Task<IActionResult> AccountLogin(GoogleLoginLogic login, AccountLogic logic, [FromBody] GoogleLoginRequest req)
    {   
        try
        {
            if (string.IsNullOrWhiteSpace(req.IdToken))
            {
                Console.WriteLine("[Auth] idToken is null or empty");
                return BadRequest(new { success = false, message = "idToken is missing" });
            }

            GoogleLoginResponse loginRes = await login.LoginAsync(req);
            UserInfoDto userInfo = await logic.GetUserInfoAsync(loginRes);

            string token = _jwtService.GenerateToken(userInfo);

            return Ok(new {
                success = true,
                token = token,
                data = userInfo
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Auth] Exception: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(500, new { success = false, message = "Internal server error" });
        }
    }

    [Authorize]
    [HttpPost("spotify")]
    public async Task<IActionResult> SpotifyLogin(SpotifyLoginLogic login,AccountLogic logic, [FromBody] SpotifyLoginRequest req)
    {
        if (string.IsNullOrEmpty(req.Code))
            return BadRequest("Missing code");

        string? uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Console.WriteLine($"[Auth] uid: {uid}");
        if (string.IsNullOrEmpty(uid))
            return Unauthorized("Missing UID in token");

        SpotifyTokenResponse tokenRes = await login.LoginAsync(req.Code);
        await logic.SetSpotifyToken(uid,tokenRes);


        return Ok(tokenRes);
        
    }

}