namespace shortener_back.Entities;

public record Shorten
{
    [Key]
    public string Code { get; init; } = String.Empty;

    public string Url { get; init; } = String.Empty;

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public long UserId { get; init; }

    public User User { get; init; } = default!;
}
