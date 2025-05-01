using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using RestApiPractice.LogicLayer;
using RestApiPractice.DataLayer.Models;
using RestApiPractice.DataLayer.Models.GoogleModel;



[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{   

    [HttpPost("google")]
    public async Task<IActionResult> GoogleLogin(GoogleLoginLogic login, AccountLogic logic, [FromBody] GoogleLoginRequest req)
    {   
        GoogleLoginResponse loginRes = await login.LoginAsync(req);
        var userInfo = await logic.GetUserInfoAsync(loginRes);

        return Ok(new {
            success = true,
            data = userInfo
        });

    }
}