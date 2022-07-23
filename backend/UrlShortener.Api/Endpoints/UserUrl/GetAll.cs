using System.Security.Claims;
using Ardalis.ApiEndpoints;
using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using UrlShortener.Api.Database;
using UrlShortener.Shared.Contracts;
using UrlShortener.Shared.Contracts.Requests;
using UrlShortener.Shared.Contracts.Responses;

namespace UrlShortener.Api.Endpoints.UserUrl;

public class Get : EndpointBaseAsync.WithRequest<GetAllUrlsRequest>.WithActionResult<GetAllUrlsResponse>
{
    private readonly AppDbContext _context;
    private readonly IHashids _hashids;

    public Get(AppDbContext context, IHashids hashids)
    {
        _context = context;
        _hashids = hashids;
    }

    [Authorize]
    [HttpGet(ApiRoutes.UserUrl.GetAll)]
    [SwaggerOperation(Tags = new[] {"UserUrl Endpoint"})]
    public override async Task<ActionResult<GetAllUrlsResponse>> HandleAsync([FromQuery] GetAllUrlsRequest req,
        CancellationToken ct = default)
    {
        var userId = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        if (!await _context.Urls.AnyAsync(ct))
            return NotFound();

        var pageResults = 5f;
        var pageCount = Math.Ceiling(await _context.Urls.LongCountAsync(ct) / pageResults);

        var urls = await _context.Urls
            .Where(x => x.UserId == userId)
            .Skip((req.Page - 1) * (int) pageResults)
            .Take((int) pageResults)
            .ToListAsync(ct);

        var urlsResponse = urls.Select(
            url => new GetUrlResponse
            {
                Id = url.Id,
                ShortUrl = _hashids.EncodeLong(url.Id),
                FullUrl = url.FullUrl
            }).ToList();

        return Ok(new GetAllUrlsResponse
        {
            Urls = urlsResponse,
            CurrentPage = req.Page,
            Pages = (int) pageCount
        });
    }
}