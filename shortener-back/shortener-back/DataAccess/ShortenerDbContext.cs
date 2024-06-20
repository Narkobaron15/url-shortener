namespace shortener_back.DataAccess;

public class ShortenerDbContext(
    DbContextOptions<ShortenerDbContext> opts
) : IdentityDbContext<User>(opts)
{
    public DbSet<Shorten> Shortens { get; set; } = default!;
    
    public DbSet<UserRefreshTokens> UserRefreshTokens { get; set; } = default!;
}
