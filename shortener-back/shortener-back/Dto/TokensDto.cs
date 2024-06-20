namespace shortener_back.Dto;

public record TokensDto(
    string AccessToken,
    string RefreshToken
);
