namespace shortener_back.Dto;

public record UserDto(
    string Username,
    string Email,
    DateTime CreatedAt,
    ICollection<ShortenDto> Shortens
)
{
    public UserDto() : this(String.Empty, String.Empty, DateTime.UtcNow, []) {}
}
