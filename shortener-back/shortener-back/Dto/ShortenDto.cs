namespace shortener_back.Dto;

public record ShortenDto(
    string Code,
    string Url,
    // string? Description,
    DateTime CreatedAt,
    DateTime? ExpiresAt,
    long Clicks
)
{
    public ShortenDto() : this(
        String.Empty,
        String.Empty,
        // String.Empty,
        DateTime.UtcNow,
        null,
        0
    ) {}
}
