namespace shortener_back.Services.Implementations;

public class ShortenService(
    IRepository<Shorten> repository,
    IMapper mapper
) : IShortenService
{
    private const int DefaultSize = 6;
    private const string Chars 
        = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    private static string GetRandString()
    {
        char[] stringChars = new char[DefaultSize];

        for (int i = 0; i < stringChars.Length; i++)
            stringChars[i] = Chars[Random.Shared.Next(Chars.Length)];

        return new string(stringChars);
    }

    private async Task<string> GetEnsuredRandString()
    {
        string code = GetRandString();
        while (await repository.Exists(code)) code = GetRandString();
        return code;
    }

    public async Task<string> GetCode(string url)
    {
        string code = await GetEnsuredRandString();
        
        await repository.Insert(new Shorten
        {
            Code = code,
            Url = url,
            UserId = 0
        });
        await repository.Save();

        return code;
    }

    public async Task<Shorten?> GetInfoInternal(string code)
        => await repository.GetById(code);
}
