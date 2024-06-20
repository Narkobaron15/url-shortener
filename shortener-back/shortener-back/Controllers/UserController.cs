using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace shortener_back.Controllers;

[ApiController, Route("user")]
public class UserController : ControllerBase
{
    [HttpGet("info"), Authorize]
    public async Task<IActionResult> GetAccountInfo()
    {
        // TODO: get user info from database and return
        return Ok();
    }

    [HttpGet("routes"), Authorize]
    public async Task<IActionResult> GetRoutesInfo()
    {
        // TODO: get user routes info and return
        return Ok();
    }

    // [HttpPost("login"), AllowAnonymous]
    // public async Task<IActionResult> Login([FromBody] LoginDto dto)
    // {
    //     // TODO: login and return tokens
    //     return Ok();
    // }
    
    // [HttpPost("register"), AllowAnonymous]
    // public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    // {
    //     // TODO: register a new user and return tokens
    //     return Ok();
    // }
}
