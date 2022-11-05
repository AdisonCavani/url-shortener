using Ardalis.ApiEndpoints;
using AutoMapper;
using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using UrlService.Contracts;
using UrlService.Contracts.Requests;
using UrlService.Contracts.Responses;
using UrlService.Services;

namespace UrlService.Endpoints;

public class Get : EndpointBaseAsync
    .WithRequest<GetRequest>
    .WithActionResult<GetResponse>
{
    private readonly IMapper _mapper;
    private readonly IHashids _hashids;
    private readonly IUrlRepository _repository;

    public Get(IMapper mapper, IHashids hashids, IUrlRepository repository)
    {
        _mapper = mapper;
        _hashids = hashids;
        _repository = repository;
    }
    
    [HttpGet(ApiRoutes.Basic.Get)]
    public override async Task<ActionResult<GetResponse>> HandleAsync(
        [FromQuery] GetRequest req,
        CancellationToken ct = default)
    {
        var id = _hashids.DecodeLong(req.ShortUrl);

        if (id.Length == 0)
            return BadRequest();

        var result = await _repository.GetUrlByIdAsync(id[0], ct);

        if (result is null)
            return NotFound();
        
        return Ok(new GetResponse
        {
            Url = result
        });
    }
}