namespace shortener_back.Services.Users;

public class UserServiceImpl(
    UserManager<User> userManager,
    IMapper mapper,
    IJwtTokenService jwtTokenService,
    ITokensRepository tokenRepository,
    ShortenerDbContext dbContext
) : IUserService
{
    public async Task<TokensDto?> Authenticate(string username, string password)
    {
        User? user = await userManager.FindByNameAsync(username);
        if (user is null ||
            !await userManager.CheckPasswordAsync(user, password)
           )
            throw new UnauthorizedAccessException(
                "Invalid username or password"
            );

        return await jwtTokenService.GenerateToken(user);
    }

    public async Task<TokensDto?> Refresh(string accessToken, string refreshToken)
    {
        ClaimsPrincipal principal = jwtTokenService
            .GetPrincipalFromExpiredToken(accessToken);
        string username = principal.Identity?.Name ?? String.Empty;
        User? user = await userManager.FindByNameAsync(username);
        if (user is null)
            throw new UnauthorizedAccessException("Invalid username");

        UserRefreshTokens? savedRefreshToken = tokenRepository
            .GetSavedRefreshTokens(username, refreshToken);
        if (savedRefreshToken is null)
            throw new UnauthorizedAccessException("Invalid refresh token");

        return await jwtTokenService.GenerateToken(user);
    }

    public async Task<UserDto> Register(
        string username,
        string email,
        string password
    )
    {
        User user = new()
        {
            UserName = username,
            Email = email
        };
        IdentityResult result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new InvalidOperationException("User creation failed");

        await userManager.AddToRoleAsync(user, "User");
        return mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetInfo(ClaimsPrincipal? user)
    {
        if (user is null) return null;

        User? userEntity = await userManager.GetUserAsync(user);
        return userEntity is null 
            ? null 
            : mapper.Map<UserDto>(userEntity) with
            {
                Shortens = await GetRoutes(userEntity)
            };
    }

    public async Task<IEnumerable<ShortenDto>?> GetRoutes(ClaimsPrincipal? user)
    {
        if (user is null) return null;

        User? userEntity = await userManager.GetUserAsync(user);
        return userEntity is null
            ? null
            : await GetRoutes(userEntity);
    }

    private async Task<IList<ShortenDto>> GetRoutes(User user)
        => mapper.Map<IList<ShortenDto>>(
            await dbContext.Shortens
                .Where(x => x.UserId == user.Id)
                .ToListAsync()
        );
}
