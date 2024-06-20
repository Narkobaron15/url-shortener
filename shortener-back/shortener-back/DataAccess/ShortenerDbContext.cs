namespace shortener_back.DataAccess;

// TODO: Add configs for records (as no tracking, etc.)

public class ShortenerDbContext(
    DbContextOptions<ShortenerDbContext> opts
) : IdentityDbContext<User>(opts)
{
    public DbSet<Shorten> Shortens { get; set; } = default!;
}
