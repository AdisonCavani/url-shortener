using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using UrlShortener.Core.Contracts.V1;
using UrlShortener.Core.Models.Requests;
using UrlShortener.WebApi.Models.App;

namespace UrlShortener.WebApi.Endpoints.Account.Email;

public class IsConfirmed : EndpointBaseAsync.WithRequest<IsEmailConfirmedDto>.WithActionResult
{
    private readonly AppDbContext _context;

    public IsConfirmed(AppDbContext context)
    {
        _context = context;
    }

    // TODO: benchmark: context vs userManager
    [HttpGet(ApiRoutes.Account.Email.IsConfirmed)]
    [SwaggerOperation(Tags = new[] {"Account Endpoint"})]
    public override async Task<ActionResult> HandleAsync([FromQuery] IsEmailConfirmedDto dto, CancellationToken ct = default)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == dto.Email, ct);

        if (user is null)
            return NotFound();

        if (user.EmailConfirmed)
            return BadRequest();

        return Ok();
    }
}