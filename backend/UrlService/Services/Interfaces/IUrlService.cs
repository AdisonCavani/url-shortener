namespace UrlService.Services.Interfaces;

public interface IUrlService
{
    Task<string?> GetUrlByIdAsync(long id, CancellationToken ct = default);
}