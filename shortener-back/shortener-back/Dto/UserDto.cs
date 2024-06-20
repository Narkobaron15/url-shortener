namespace shortener_back.Dto;

public record UserDto(
    string Username,
    string Email,
    ICollection<ShortenDto> Shortens
);
