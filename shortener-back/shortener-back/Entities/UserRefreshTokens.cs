namespace shortener_back.Entities;

public class UserRefreshTokens
{
    [Key] public long Id { get; set; }
    [Required, MaxLength(500)] public required string UserName { get; set; } = String.Empty;
    [Required, MaxLength(2000)] public required string RefreshToken { get; set; } = String.Empty;
    public bool IsActive { get; set; } = true;
} 