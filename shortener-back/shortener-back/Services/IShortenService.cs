namespace shortener_back.Services;

public interface IShortenService
{
    public Task<string> GetCode(string url);
    public Task<string?> GetUrl(string? code);
    // public Task<ShortenDto> GetInfo(string code, ClaimsPrincipal? user);
    // public Task<IEnumerable<ShortenDto>> GetRange(int pageNumber, int pageSize, ClaimsPrincipal? user);
}
