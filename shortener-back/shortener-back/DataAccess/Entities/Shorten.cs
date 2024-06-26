﻿namespace shortener_back.DataAccess.Entities;

public class Shorten
{
    [Key, MaxLength(20)] public string Code { get; set; } = String.Empty;

    [MaxLength(1000)] public string Url { get; set; } = String.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(300)] public string UserId { get; set; } = String.Empty;

    public User? User { get; set; }

    public long Clicks { get; set; }
    
    public DateTime? ExpiresAt { get; set; }
}
