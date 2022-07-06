using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UrlShortener.Core;
using UrlShortener.Core.Contracts.V1;
using UrlShortener.Core.Models.Requests;
using UrlShortener.WebApi.Models.Entities;
using UrlShortener.WebApi.Services;

namespace UrlShortener.WebApi.Endpoints.Account.Email;

public class ResendVerification : EndpointBaseAsync.WithRequest<ResendVerificationEmailDto>.WithActionResult
{
    private readonly EmailHandler _emailHandler;
    private readonly UserManager<AppUser> _userManager;

    public ResendVerification(EmailHandler emailHandler, UserManager<AppUser> userManager)
    {
        _emailHandler = emailHandler;
        _userManager = userManager;
    }

    [HttpGet(ApiRoutes.Account.Email.ResendVerification)]
    [SwaggerOperation(Tags = new[] {"Account Endpoint"})]
    public override async Task<ActionResult> HandleAsync([FromQuery] ResendVerificationEmailDto dto, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
            return BadRequest(new ErrorResponse
            {
                Errors = new[] {"Couldn't find user associated with this email"}
            });

        var emailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

        if (emailConfirmed)
            return Conflict(new ErrorResponse
            {
                Errors = new[] {"Email is already confirmed"}
            });

        var result = await _emailHandler.SendVerificationEmailAsync(user, ct);

        return result
            ? Ok()
            : StatusCode(500);
    }
}