namespace shortener_back.Services.Security;

public interface ITokensRepository
{
    Task<bool> IsValidUserAsync(string username, string password);
	
    Task<UserRefreshTokens> AddUserRefreshTokens(UserRefreshTokens tokens);

    UserRefreshTokens? GetSavedRefreshTokens(string username, string refreshToken);

    Task DeleteUserRefreshTokens(string username, string refreshToken);
}
