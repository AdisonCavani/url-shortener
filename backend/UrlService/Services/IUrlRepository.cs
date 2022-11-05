namespace UrlService.Services;

public interface IUrlRepository
{
    Task<string?> GetUrlByIdAsync(long id, CancellationToken ct = default);
}