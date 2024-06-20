using Microsoft.AspNetCore.Mvc;

namespace shortener_back.Controllers;

[ApiController, Route("")/*, Authorize*/]
public class ShortenController(
    IConfiguration configuration
) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> Shorten([FromBody] string url)
    {
        // call service and add to database
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        string mainPage = configuration["MainPage"]
                          ?? String.Empty;
        return RedirectPermanent(mainPage);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> Get(string code)
    {
        // call service and redirect
        // string url = await urlService.Get(code);
        string url = "https://www.google.com";
        return RedirectPermanent(url);
    }
}
