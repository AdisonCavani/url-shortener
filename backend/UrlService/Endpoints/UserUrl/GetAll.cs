using System.Security.Claims;
using Ardalis.ApiEndpoints;
using AutoMapper;
using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using UrlService.Database;
using UrlShortener.Shared.Contracts;
using UrlShortener.Shared.Contracts.Dtos;
using UrlShortener.Shared.Contracts.Requests;
using UrlShortener.Shared.Contracts.Responses;

namespace UrlService.Endpoints.UserUrl;

public class GetAll : EndpointBaseAsync.WithRequest<GetAllUserUrlsRequest>.WithActionResult<GetAllUserUrlsResponse>
{
    private readonly IMapper _mapper;
    private readonly IHashids _hashids;
    private readonly AppDbContext _context;

    public GetAll(IMapper mapper, IHashids hashids, AppDbContext context)
    {
        _mapper = mapper;
        _hashids = hashids;
        _context = context;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    [HttpGet(ApiRoutes.UserUrl.GetAll)]
    [SwaggerOperation(Tags = new[] {"UserUrl Endpoint"})]
    public override async Task<ActionResult<GetAllUserUrlsResponse>> HandleAsync(
        [FromQuery] GetAllUserUrlsRequest req,
        CancellationToken ct = default)
    {
        var userId = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        if (!await _context.Urls.AnyAsync(ct))
            return NoContent();

        var pageResults = 5f;
        var urlsCount = await _context.Urls
            .Where(x => x.UrlDetails != null && x.UrlDetails.UserId == userId)
            .LongCountAsync(ct);
        var pageCount = Math.Ceiling(urlsCount / pageResults);

        if (req.Page > pageCount)
            return NotFound();

        var urls = await _context.Urls
            .Include(x => x.UrlDetails)
            .Where(x => x.UrlDetails != null && x.UrlDetails.UserId == userId)
            .Skip((req.Page - 1) * (int) pageResults)
            .Take((int) pageResults)
            .Include(x => x.UrlDetails!.Tags)
            .ToListAsync(ct);

        var urlsResponse = urls.Select(
            url => new GetUserUrlResponse
            {
                Id = url.Id,
                ShortUrl = _hashids.EncodeLong(url.Id),
                FullUrl = url.FullUrl,
                UrlDetails = _mapper.Map<UrlDetailsDto>(url.UrlDetails)
            }).ToList().OrderBy(x => x.Id);

        return Ok(new GetAllUserUrlsResponse
        {
            Urls = urlsResponse,
            CurrentPage = req.Page,
            Pages = (int) pageCount
        });
    }
}