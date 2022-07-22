namespace UrlShortener.Api.Services;

public interface IUrlService
{
    Task<string?> GetUrlByIdAsync(int id, CancellationToken ct = default);

    Task<string?> GetCustomUrlAsync(string shortUrl, CancellationToken ct = default);
}