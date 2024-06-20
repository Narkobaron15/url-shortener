namespace shortener_back.Services.Declarations;

public interface IShortenService
{
    public Task<string> GetCode(string url);
    // public Task<ShortenDto> GetInfoExport(string code, ClaimsPrincipal? user);
    public Task<Shorten?> GetInfoInternal(string code);
}
