namespace shortener_back.Services.Security;

public interface IJwtTokenService
{
    Task<TokensDto?> GenerateToken(User? user);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}