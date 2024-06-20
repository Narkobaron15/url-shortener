namespace shortener_back.Controllers;

[ApiController, Route("")]
public class ShortenController(
    IShortenService urlService,
    IConfiguration configuration
) : ControllerBase
{
    [HttpPut("shorten"), Authorize]
    public async Task<IActionResult> Shorten([FromBody] string url)
    {
        string code = await urlService.GetCode(url, User);
        return Ok(configuration["ShortenPage"] + code);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> Get(string? code)
    {
        string? mainPage = configuration["MainPage"];

        if (mainPage is null) 
            throw new InvalidOperationException("MainPage is not set");
        
        string url = await urlService.GetUrl(code) ?? mainPage;
        return RedirectPermanent(url);
    }
}
