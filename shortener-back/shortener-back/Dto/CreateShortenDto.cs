namespace shortener_back.Dto;

public record CreateShortenDto(
    string Url,
    DateTime? ExpiresAt = null
    // string? Description
);
