using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using RestApiPractice.LogicLayer;
using RestApiPractice.DataLayer.Models;
using System.Text;


[Route("api/[controller]")]
[Authorize]
public class SpotifyController : ControllerBase
{   


    private readonly SpotifyApiLogic _logic;

    public SpotifyController(SpotifyApiLogic logic)
    {
        _logic = logic;
    }

    [HttpGet("token")]
    public async Task<IActionResult> GetToken()
    {   
        var token = await _logic.GetUserSpotifyTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
            return NoContent();                       
            
        return Ok(new { access_token = token });
    }

    [HttpPost("unbind")]
    public async Task<IActionResult> UnbindSpotify()
    {
        var success = await _logic.ClearSpotifyTokensAsync();

        if (success)
            return Ok(new { message = "unbind success" });

        return StatusCode(502, new { message = "Failed to transfer playback to Spotify device" });
    }

    [HttpPut("transfer-playback")]
    public async Task<IActionResult> TransferPlayback([FromBody] TransferPlaybackRequest req)
    {   
        var play = req.Play ?? false;
        var success = await _logic.TransferPlaybackAsync(req.DeviceId, play);

        if (success)
            return Ok(new { message = "Playback control transferred" });

        return StatusCode(502, new { message = "Failed to transfer playback to Spotify device" });
    }

    [HttpPut("load-track")]
    public async Task<IActionResult> LoadTrackToPlayer([FromBody] LoadTrackRequest req)
    {   
        var success = await _logic.LoadTrack(req.DeviceId, req.TrackUri);

        if (success)
            return Ok(new { message = "ttttt" });

        return StatusCode(502, new { message = "無法加載歌曲" });
    }

}