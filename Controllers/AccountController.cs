using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> GoogleLogin(GoogleLoginLogic login, AccountLogic logic, [FromBody] GoogleLoginRequest req)
    {   
        GoogleLoginResponse loginRes = await login.LoginAsync(req);
        UserInfoDto userInfo = await logic.GetUserInfoAsync(loginRes);

        string token = _jwtService.GenerateToken(userInfo);

        return Ok(new {
            success = true,
            token = token,
            data = userInfo
        });

    }
}