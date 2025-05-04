using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


[Route("api/[controller]")]
[Authorize]
public class TestController : ControllerBase
{   
    [HttpGet("test")]
    public IActionResult Test()
    {

        Console.WriteLine(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        Console.WriteLine(User.FindFirst(ClaimTypes.Email)?.Value);
        Console.WriteLine(User.FindFirst(ClaimTypes.Role)?.Value);
        return Ok(new
        {
            success = true,
            message = "Test successful"
        });
    }
}