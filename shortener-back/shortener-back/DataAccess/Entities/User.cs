namespace shortener_back.DataAccess.Entities;

public class User : IdentityUser
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public IList<Shorten> Shortens { get; init; } = [];
}
