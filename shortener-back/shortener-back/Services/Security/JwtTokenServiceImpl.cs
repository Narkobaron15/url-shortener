namespace shortener_back.Services.Security;

public class JwtTokenServiceImpl(
    IConfiguration configuration,
    UserManager<User> userManager
) : IJwtTokenService
{
    public async Task<TokensDto?> GenerateToken(User? user)
    {
        if (user == null) return null;

        return new TokensDto
        (
            await CreateAccessToken(user),
            CreateRefreshToken()
        );
    }
    
    private readonly string _secretKey = configuration["JwtSecretKey"]
        ?? throw new ApplicationException("JwtSecretKey is not set");

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        byte[] key = Encoding.UTF8.GetBytes(_secretKey);

        TokenValidationParameters tokenValidationParameters = new()
        {
            ValidateAudience = false, // on production make true
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidIssuer = configuration["JwtIssuer"],
            ClockSkew = TimeSpan.Zero
        };

        JwtSecurityTokenHandler tokenHandler = new();
        ClaimsPrincipal? principal = tokenHandler.ValidateToken(
            token, tokenValidationParameters, out SecurityToken? securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

    private async Task<string> CreateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? String.Empty),
            new(ClaimTypes.Email, user.Email ?? String.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var roles = await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(
            role => new Claim(ClaimTypes.Role, role)
        ));
        
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_secretKey));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwt = new(
            signingCredentials: credentials,
            claims: claims,
            expires: DateTime.Now.AddDays(15),
            issuer: configuration["JwtIssuer"]
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private static string CreateRefreshToken()
    {
        byte[] randomNumber = new byte[32];
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
