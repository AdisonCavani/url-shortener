using System.Security.Claims;
using Ardalis.ApiEndpoints;
using AutoMapper;
using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UrlService.Database;
using UrlService.Database.Entities;
using UrlShortener.Shared.Contracts;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlService.Endpoints.UserUrl;

public class Save : EndpointBaseAsync.WithRequest<SaveUserUrlRequest>.WithActionResult<string>
{
    private readonly IMapper _mapper;
    private readonly IHashids _hashids;
    private readonly AppDbContext _context;

    public Save(IMapper mapper, IHashids hashids, AppDbContext context)
    {
        _mapper = mapper;
        _context = context;
        _hashids = hashids;
    }
    
    [Authorize]
    [HttpPost(ApiRoutes.UserUrl.Save)]
    [SwaggerOperation(Tags = new[] { "UserUrl Endpoint" })]
    public override async Task<ActionResult<string>> HandleAsync(SaveUserUrlRequest req, CancellationToken ct = default)
    {
        var userId = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        Database.Entities.Url obj = new()
        {
            FullUrl = req.Url,
            UrlDetails = new UrlDetails
            {
                UserId = userId,
                Title = req.Title,
                Tags = _mapper.Map<List<Tag>>(req.Tags)
            }
        };

        await _context.Urls.AddAsync(obj, ct);
        var saved = await _context.SaveChangesAsync(ct);

        var encodedId = _hashids.EncodeLong(obj.Id);

        var createdObj = new ObjectResult(encodedId)
        {
            StatusCode = StatusCodes.Status201Created
        };

        return saved > 0 ? createdObj : StatusCode(StatusCodes.Status500InternalServerError);
    }
}