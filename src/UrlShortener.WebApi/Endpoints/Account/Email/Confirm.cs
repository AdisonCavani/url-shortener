using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UrlShortener.Core;
using UrlShortener.Core.Contracts.V1;
using UrlShortener.Core.Models.Requests;
using UrlShortener.WebApi.Models.Entities;

namespace UrlShortener.WebApi.Endpoints.Account.Email;

public class Confirm : EndpointBaseAsync.WithRequest<ConfirmEmailDto>.WithActionResult
{
    private readonly UserManager<AppUser> _userManager;

    public Confirm(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet(ApiRoutes.Account.Email.Confirm)]
    [SwaggerOperation(Tags = new[] { "Account Endpoint" })]
    public override async Task<ActionResult> HandleAsync([FromQuery] ConfirmEmailDto dto, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
            return NotFound(new ErrorResponse
            {
                Errors = new[] { "Couldn't find user associated with this email" }
            });

        var emailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

        if (emailConfirmed)
            return Conflict(new ErrorResponse
            {
                Errors = new[] { "Email is already confirmed" }
            });

        var result = await _userManager.ConfirmEmailAsync(user, dto.Token);

        return result.Succeeded
            ? Ok()
            : BadRequest(new ErrorResponse
            {
                Errors = result.Errors.Select(x => x.Description)
            });
    }
}