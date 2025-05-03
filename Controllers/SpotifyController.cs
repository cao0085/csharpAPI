using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;



namespace RestApiPractice.Controllers.SpotifyController
{
    // [Route("[controller]")]
    // [Authorize]
    // [ApiController]
    // public class SpotifyController : ControllerBase
    // {   

    //     private readonly SpotifLogic _logic;

    //     public SpotifyController(SpotifLogic logic)
    //     {
    //         _logic = logic;
    //     }

    //     [HttpPost("callback")]
    //     public async Task<IActionResult> SpotifyCallback([FromBody] SpotifyCodeDto dto)
    //     {
    //         if (string.IsNullOrEmpty(dto.Code))
    //             return BadRequest("Missing Spotify code");

    //         var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //         if (uid == null) return Unauthorized();

    //         var tokenRes = await _logic.LoginAsync(uid, dto.Code);

    //         return Ok(new { success = true, token = tokenRes.AccessToken });
    //     }
    // }
}

