using Ardalis.ApiEndpoints;
using HashidsNet;
using ManagementService.Contracts;
using ManagementService.Contracts.Requests;
using ManagementService.Contracts.Responses;
using ManagementService.Database;
using ManagementService.Database.Entities;
using ManagementService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagementService.Endpoints;

public class Create : EndpointBaseAsync
    .WithRequest<CreateRequest>
    .WithActionResult<CreateResponse>
{
    private readonly IHashids _hashids;
    private readonly AppDbContext _context;
    private readonly IMessageBusPublisher _publisher;

    public Create(IHashids hashids, AppDbContext context, IMessageBusPublisher publisher)
    {
        _hashids = hashids;
        _context = context;
        _publisher = publisher;
    }
    
    // [Authorize]
    [HttpPost(ApiRoutes.Basic.Create)]
    public override async Task<ActionResult<CreateResponse>> HandleAsync(
        CreateRequest req,
        CancellationToken ct = default)
    {
        // var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //
        // if (userId is null)
        //     return StatusCode(StatusCodes.Status500InternalServerError);

        var userId = "2ac243cd-2d56-410a-936f-b07274a75d83";

        var entity = _mapper.Map<DetailsEntity>(req);

        await _context.Details.AddAsync(entity, ct);
        var saved = await _context.SaveChangesAsync(ct);

        _publisher.PublishUrlCreatedEvent(new()
        {
            UrlId = entity.Id,
            UserId = userId,
            Title = req.Title,
            Tags = req.Tags
        });

        if (saved <= 0)
            return StatusCode(StatusCodes.Status500InternalServerError);

        return new ObjectResult(new CreateResponse {ShortUrl = _hashids.EncodeLong(entity.Id)})
        {
            StatusCode = StatusCodes.Status201Created
        };
    }
}