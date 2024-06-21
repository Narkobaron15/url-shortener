namespace shortener_back.Controllers;

[ApiController, Route("[controller]")]
public class UserController(
    IUserService userService,
    IShortenService shortenService,
    IWebHostEnvironment env,
    IConfiguration configuration
) : ControllerBase
{
    private readonly CookieOptions _options = new()
    {
        HttpOnly = true,
        SameSite = SameSiteMode.None,
        Secure = true,
        MaxAge = TimeSpan.FromMinutes(
            Convert.ToInt32(env.IsDevelopment()
                ? configuration["JwtExpireMinutes"]
                : Environment.GetEnvironmentVariable("JwtExpireMinutes")
            )
        ),
    };

    private IActionResult ProcessTokens(TokensDto? tokens)
    {
        if (tokens is null)
            return BadRequest();

        Response.Cookies.Append("jwt", tokens.AccessToken, _options);
        Response.Cookies.Append("refresh", tokens.RefreshToken, _options);
        return Ok(tokens);
    }

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
        return ProcessTokens(tokens);
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

    [HttpPost("refresh"), AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] TokensDto dto)
    {
        TokensDto? tokens = await userService.Refresh(
            dto.AccessToken,
            dto.RefreshToken
        );
        return ProcessTokens(tokens);
    }
}
