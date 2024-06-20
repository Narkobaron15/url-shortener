namespace shortener_back.Services.Shorten;

public interface IShortenService
{
    public Task<string> GetCode(string url, ClaimsPrincipal? user);
    public Task<string?> GetUrl(string? code);
    public Task<ShortenDto> GetInfo(string code, ClaimsPrincipal? user);
    public Task<IEnumerable<ShortenDto>> GetRange(int pageNumber, int pageSize, ClaimsPrincipal? user);
}
