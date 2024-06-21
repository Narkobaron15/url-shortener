namespace shortener_back.Controllers;

[ApiController, Route("")]
public class ShortenController(IShortenService urlService) : ControllerBase
{
    [HttpPut("shorten"), Authorize]
    public async Task<IActionResult> Shorten([FromBody] string url)
    {
        string shortenUrl = await urlService.GetShortenUrl(url, User);
        return Ok(shortenUrl);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> Get(string? code)
    {
        string url = await urlService.GetUrl(code);
        return RedirectPermanent(url);
    }

    [HttpGet("/")]
    public async Task<IActionResult> Index() => await Get(null);
}
