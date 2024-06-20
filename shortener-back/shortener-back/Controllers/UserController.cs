namespace shortener_back.Controllers;

[ApiController, Route("user")]
public class UserController(
    IUserService userService,
    IShortenService shortenService
) : ControllerBase
{
    [HttpGet("info"), Authorize]
    public async Task<IActionResult> GetAccountInfo()
    {
        UserDto? user = await userService.GetInfo(User);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpGet("routes"), Authorize]
    public async Task<IActionResult> GetRoutesInfo(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10
    )
    {
        if (pageNumber < 1 || pageSize < 1)
            return BadRequest();

        UserDto? user = await userService.GetInfo(User);

        if (user is null)
            return Unauthorized();

        if (user.Shortens.Count == 0)
            return NotFound();

        return Ok(await shortenService.GetRange(pageNumber, pageSize, User));
    }

    [HttpGet("route/{code}"), Authorize]
    public async Task<IActionResult> GetRouteInfo(string code)
    {
        ShortenDto? route = await shortenService.GetInfo(code, User);
        return route is null ? NotFound() : Ok(route);
    }

    [HttpDelete("route/{code}"), Authorize]
    public async Task<IActionResult> DeleteRoute(string code)
    {
        bool result = await shortenService.DeleteCode(code, User);
        return result ? Ok() : BadRequest();
    }

    [HttpPost("login"), AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        TokensDto? tokens = await userService.Authenticate(
            dto.Username,
            dto.Password
        );
        return tokens is null ? BadRequest() : Ok(tokens);
    }

    [HttpPost("register"), AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        UserDto result = await userService.Register(
            dto.Username,
            dto.Email,
            dto.Password
        );
        return Ok(result);
    }
    
    [HttpPost("refresh"), Authorize]
    public async Task<IActionResult> Refresh([FromBody] TokensDto dto)
    {
        TokensDto? tokens = await userService.Refresh(dto.AccessToken, dto.RefreshToken);
        return tokens is null ? BadRequest() : Ok(tokens);
    }
}
