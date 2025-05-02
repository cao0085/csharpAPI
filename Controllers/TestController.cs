using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Text.Json;


[Route("api/[controller]")]
public class TestController : ControllerBase
{

    private readonly IConfiguration _configuration;

    public TestController(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok(new
        {
            success = true,
            message = "Test successful"
        });
    }

    [HttpGet("test/env")]
    public IActionResult TestEnv()
    {


        var jwtConfig = new
        {
            Key = _configuration["Jwt:Key"],
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        return Ok(new
        {
            success = true,
            firebase = credentialJson,
            jwt = jwtConfig
        });
    }
}


