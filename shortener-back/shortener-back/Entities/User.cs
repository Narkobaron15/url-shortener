namespace shortener_back.Entities;

public class User : IdentityUser
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public required IList<Shorten> Shortens { get; init; }
}
