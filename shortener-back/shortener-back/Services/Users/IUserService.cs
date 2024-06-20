namespace shortener_back.Services.Users;

public interface IUserService
{
    Task<TokensDto?> Authenticate(string username, string password);
    Task<TokensDto?> Refresh(string accessToken, string refreshToken);
    Task<UserDto> Register(string username, string email, string password);
    Task<UserDto?> GetInfo(ClaimsPrincipal? user);
    Task<IEnumerable<ShortenDto>?> GetRoutes(ClaimsPrincipal? user);
    // Task<UserDto> UpdateInfo(ClaimsPrincipal user, string username, string email);
    // Task<UserDto> UpdatePassword(ClaimsPrincipal user, string oldPassword, string newPassword);
    // Task Delete(ClaimsPrincipal user);
}
