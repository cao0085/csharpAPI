using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using RestApiPractice.LogicLayer;


[Route("api/[controller]")]
[Authorize]
public class SpotifyController : ControllerBase
{   


    private readonly SpotifyApiLogic _logic;

    public SpotifyController(SpotifyApiLogic logic)
    {
        _logic = logic;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentSpotifyUser()
    {
        var email = await _logic.GetSpotifyEmailAsync();
        if (string.IsNullOrEmpty(email)) return Unauthorized("無法取得 Spotify 資料");

        return Ok(new { Email = email });
    }


}