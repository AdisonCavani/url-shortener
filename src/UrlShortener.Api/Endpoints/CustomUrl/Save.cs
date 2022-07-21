﻿using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using UrlShortener.Api.Models.App;
using UrlShortener.Core.Contracts.V1;
using UrlShortener.Core.Models.Requests;

namespace UrlShortener.Api.Endpoints.CustomUrl;

public class Save : EndpointBaseAsync.WithRequest<CustomUrlDto>.WithActionResult
{
    private readonly AppDbContext _context;

    public Save(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpPost(ApiRoutes.CustomUrl.Save)]
    [SwaggerOperation(Tags = new[] {"CustomUrl Endpoint"})]
    public override async Task<ActionResult> HandleAsync([FromBody] CustomUrlDto dto, CancellationToken ct = default)
    {
        var uid = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(uid))
            return Unauthorized();

        var existInDb = await _context.CustomUrls.AnyAsync(e => e.ShortUrl == dto.ShortUrl, ct);
        if (existInDb)
            return Conflict();

        var obj = new Models.Entities.CustomUrl()
        {
            FullUrl = dto.FullUrl,
            ShortUrl = dto.ShortUrl,
            UserId = uid
        };

        await _context.AddAsync(obj, ct);
        var saved = await _context.SaveChangesAsync(ct);

        return saved > 0 ? new StatusCodeResult(201) : StatusCode(500);
    }
}