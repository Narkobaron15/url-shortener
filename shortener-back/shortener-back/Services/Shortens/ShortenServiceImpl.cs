namespace shortener_back.Services.Shortens;

public class ShortenServiceImpl(
    IRepository<Shorten> repository,
    UserManager<User> userManager,
    IWebHostEnvironment env,
    IConfiguration configuration,
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

    private async Task<bool> IsExpiredAndDeleted(Shorten entry)
    {
        if (entry.ExpiresAt is null || entry.ExpiresAt > DateTime.UtcNow)
            return false;

        await repository.Delete(entry);
        await repository.Save();
        return true;
    }

    private async Task<User?> FindUser(ClaimsPrincipal? uPrincipal)
    {
        if (uPrincipal is null) return null;
        return await userManager.GetUserAsync(uPrincipal);
    }

    private async Task<bool> IsUserAdmin(ClaimsPrincipal? uPrincipal)
    {
        User? user = await FindUser(uPrincipal);
        if (user is null) return false;

        var roles = await userManager.GetRolesAsync(user);
        return roles.Contains("Admin");
    }

    private async Task<bool> IsUserOwner(
        ClaimsPrincipal? uPrincipal,
        string code
    )
    {
        User? user = await FindUser(uPrincipal);
        if (user is null) return false;

        Shorten? entry = await repository.GetById(code);
        return entry?.UserId == user.Id;
    }

    public async Task<string> GetCode(string url, ClaimsPrincipal? user)
    {
        // check if string is a valid URL
        if (!Uri.TryCreate(url, UriKind.Absolute, out _))
            throw new ArgumentException("URL is not valid", nameof(url));

        User? u = await FindUser(user);
        if (u is null)
            throw new ArgumentException("User is not found", nameof(user));

        string code = await GetEnsuredRandString();

        await repository.Insert(new Shorten
        {
            Code = code,
            Url = url,
            UserId = u.Id
        });
        await repository.Save();

        return code;
    }

    public async Task<string> GetShortenUrl(string url, ClaimsPrincipal? user)
    {
        string? code = await GetCode(url, user),
            host = env.IsDevelopment()
                ? configuration["ShortenerPage"]
                : Environment.GetEnvironmentVariable("ShortenerPage");

        if (host is null)
            throw new InvalidOperationException("ShortenerPage is not set");

        return $"{host}/{code}";
    }

    public async Task<bool> DeleteCode(string code, ClaimsPrincipal? user)
    {
        if (await FindUser(user) is null)
            return false;

        Shorten? entry = await repository.GetById(code);
        if (entry is null)
            return false;

        if (entry.UserId != String.Empty
            && !await IsUserAdmin(user)
            && !await IsUserOwner(user, code)
           )
            return false;

        await repository.Delete(entry);
        await repository.Save();
        return true;
    }

    public async Task<string> GetUrl(string? code)
    {
        string? mainPage = env.IsDevelopment()
            ? configuration["MainPage"]
            : Environment.GetEnvironmentVariable("MainPage");

        if (mainPage is null)
            throw new InvalidOperationException("MainPage is not set");

        if (code is null) return mainPage;

        Shorten? entry = await repository.GetById(code);
        if (entry is null) return mainPage;

        if (await IsExpiredAndDeleted(entry))
            return mainPage;

        entry.Clicks++;
        await repository.Update(entry);
        await repository.Save();

        return entry.Url;
    }

    public async Task<ShortenDto?> GetInfo(string code, ClaimsPrincipal? user)
    {
        if (await FindUser(user) is null)
            throw new UnauthorizedAccessException("User is not found");

        Shorten? entry = await repository.GetById(code);
        if (entry is null)
            return null;

        if (await IsExpiredAndDeleted(entry))
            throw new ArgumentException(
                "Shorten entry is expired",
                nameof(code)
            );

        if (entry.UserId != String.Empty
            && !await IsUserAdmin(user)
            && !await IsUserOwner(user, code)
           )
            throw new UnauthorizedAccessException(
                "This user doesn't owns this record"
            );

        return mapper.Map<ShortenDto>(entry);
    }

    public async Task<IEnumerable<ShortenDto>?> GetRange(
        int pageNumber,
        int pageSize,
        ClaimsPrincipal? user
    )
    {
        User? u = await FindUser(user);
        if (u is null)
            throw new ArgumentException("User is not found", nameof(user));

        var entries = await repository.GetRange(
            pageNumber,
            pageSize,
            x => x.UserId == u.Id
        );
        return mapper.Map<IEnumerable<ShortenDto>>(entries);
    }
}
