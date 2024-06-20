namespace shortener_back.Dto;

public record UserDto(
    string Username,
    string Email,
    DateTime CreatedAt,
    ICollection<ShortenDto> Shortens
);
