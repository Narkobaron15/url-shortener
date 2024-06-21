namespace shortener_back.Dto;

public record RegisterDto(
    string Username = "",
    string Email = "",
    string Password = ""
);
