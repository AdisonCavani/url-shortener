using Microsoft.EntityFrameworkCore;
using UrlService.Database;
using UrlService.Services.Interfaces;

namespace UrlService.Services;

public class UrlService : IUrlService
{
    private readonly AppDbContext _context;

    public UrlService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string?> GetUrlByIdAsync(long id, CancellationToken ct = default)
    {
        var result = await _context.Urls.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, ct);
        return result?.FullUrl;
    }
}