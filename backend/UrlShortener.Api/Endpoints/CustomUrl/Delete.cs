using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Database;
using UrlShortener.Shared.Contracts;

namespace UrlShortener.Api.Endpoints.CustomUrl;

public class Delete: EndpointBaseAsync.WithRequest<string>.WithActionResult
{
    private readonly AppDbContext _context;
    
    public Delete(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpDelete(ApiRoutes.CustomUrl.Delete)]
    public override async Task<ActionResult> HandleAsync(string dto, CancellationToken ct = default)
    {
        var exist = await _context.CustomUrls.FirstOrDefaultAsync(x => x.ShortUrl == dto, ct);

        if (exist is null)
            return NotFound();

        _context.CustomUrls.Remove(exist);
        var result = await _context.SaveChangesAsync(ct);

        return result > 0 ? Ok() : new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}