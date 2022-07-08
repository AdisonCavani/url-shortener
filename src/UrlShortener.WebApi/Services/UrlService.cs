using Microsoft.EntityFrameworkCore;
using UrlShortener.WebApi.Models.App;
using UrlShortener.WebApi.Services.Interfaces;

namespace UrlShortener.WebApi.Services;

public class UrlService : IUrlService
{
    private readonly AppDbContext _context;

    public UrlService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string?> GetUrlByIdAsync(int id, CancellationToken ct = default)
    {
        var result = await _context.Urls.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, ct);
        return result?.FullUrl;
    }

    public async Task<string?> GetCustomUrlAsync(string shortUrl, CancellationToken ct = default)
    {
        var result = await _context.CustomUrls.AsNoTracking().FirstOrDefaultAsync(a => a.ShortUrl == shortUrl, ct);
        return result?.FullUrl;
    }
}