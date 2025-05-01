using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;



[Route("api/[controller]")]
public class TestController : ControllerBase
{   
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok(new
        {
            success = true,
            message = "Test successful"
        });
    }
}