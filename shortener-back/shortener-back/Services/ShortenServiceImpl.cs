namespace shortener_back.Services;

public class ShortenServiceImpl(
    IRepository<Shorten> repository,
    UserManager<User> userManager,
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
            UserId = String.Empty
        });
        await repository.Save();

        return code;
    }

    public async Task<string?> GetUrl(string? code)
    {
        if (code is null) return null;

        Shorten? entry = await repository.GetById(code);
        if (entry is null) return null;
        
        if (entry.ExpiresAt is not null && entry.ExpiresAt < DateTime.UtcNow)
        {
            await repository.Delete(entry);
            await repository.Save();
            return null;
        }

        entry.Clicks++;
        await repository.Update(entry);
        await repository.Save();

        return entry.Url;
    }
}
