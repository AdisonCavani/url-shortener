using Ardalis.ApiEndpoints;
using AutoMapper;
using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using UrlService.Contracts;
using UrlService.Contracts.Requests;
using UrlService.Contracts.Responses;
using UrlService.Database;
using UrlService.Database.Entities;

namespace UrlService.Endpoints;

public class Create : EndpointBaseAsync
    .WithRequest<CreateRequest>
    .WithActionResult<CreateResponse>
{
    private readonly IMapper _mapper;
    private readonly IHashids _hashids;
    private readonly AppDbContext _context;

    public Create(IMapper mapper, IHashids hashids, AppDbContext context)
    {
        _mapper = mapper;
        _hashids = hashids;
        _context = context;
    }

    [HttpPost(ApiRoutes.Basic.Create)]
    public override async Task<ActionResult<CreateResponse>> HandleAsync(
        CreateRequest req,
        CancellationToken ct = default)
    {
        var entity = _mapper.Map<UrlEntity>(req);
        await _context.AddAsync(entity, ct);
        var result = await _context.SaveChangesAsync(ct);

        if (result <= 0)
            return StatusCode(StatusCodes.Status500InternalServerError);

        var res = new CreateResponse {ShortUrl = _hashids.EncodeLong(entity.Id)};
        return new ObjectResult(res)
        {
            StatusCode = StatusCodes.Status201Created
        };
    }
}