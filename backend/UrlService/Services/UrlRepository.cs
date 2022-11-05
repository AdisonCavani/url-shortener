using Microsoft.EntityFrameworkCore;
using UrlService.Database;

namespace UrlService.Services;

public class UrlRepository : IUrlRepository
{
    private readonly AppDbContext _context;

    public UrlRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string?> GetUrlByIdAsync(long id, CancellationToken ct = default)
    {
        var result = await _context.Urls.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, ct);
        return result?.FullUrl;
    }
}