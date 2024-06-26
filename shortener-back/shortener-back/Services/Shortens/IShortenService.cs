﻿namespace shortener_back.Services.Shortens;

public interface IShortenService
{
    public Task<string> GetCode(CreateShortenDto dto, ClaimsPrincipal? user);
    public Task<string> GetShortenUrl(CreateShortenDto dto, ClaimsPrincipal? user);
    public Task<bool> DeleteCode(string code, ClaimsPrincipal? user);
    public Task<string> GetUrl(string? code);
    public Task<ShortenDto?> GetInfo(string code, ClaimsPrincipal? user);
    public Task<IEnumerable<ShortenDto>?> GetRange(int pageNumber, int pageSize, ClaimsPrincipal? user);
}
