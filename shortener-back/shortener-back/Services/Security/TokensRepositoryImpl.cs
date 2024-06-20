namespace shortener_back.Services.Security;

public class TokensRepositoryImpl(ShortenerDbContext context) : ITokensRepository
{
    public async Task<bool> IsValidUserAsync(string username, string password)
    {
        UserRefreshTokens? user = await context.UserRefreshTokens
            .FirstOrDefaultAsync(u => u.UserName == username && u.RefreshToken == password);
        return user != null;
    }

    public async Task<UserRefreshTokens> AddUserRefreshTokens(UserRefreshTokens tokens)
    {
        await context.UserRefreshTokens.AddAsync(tokens);
        await context.SaveChangesAsync();
        return tokens;
    }

    public UserRefreshTokens? GetSavedRefreshTokens(string username, string refreshToken)
    {
        return context.UserRefreshTokens
            .FirstOrDefault(u => u.UserName == username && u.RefreshToken == refreshToken);
    }

    public async Task DeleteUserRefreshTokens(string username, string refreshToken)
    {
        UserRefreshTokens? user = context.UserRefreshTokens
            .FirstOrDefault(u => u.UserName == username && u.RefreshToken == refreshToken);
        if (user != null)
        {
            context.UserRefreshTokens.Remove(user);
            await context.SaveChangesAsync();
        }
    }
    
}
