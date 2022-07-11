using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UrlShortener.Core;
using UrlShortener.Core.Contracts.V1;
using UrlShortener.Core.Models.Requests;
using UrlShortener.WebApi.Models.Entities;
using UrlShortener.WebApi.Services.Interfaces;

namespace UrlShortener.WebApi.Endpoints.Account.Password;

public class Recover : EndpointBaseAsync.WithRequest<PasswordRecoveryDto>.WithActionResult
{
    private readonly IEmailHandler _emailHandler;
    private readonly UserManager<AppUser> _userManager;

    public Recover(IEmailHandler emailHandler, UserManager<AppUser> userManager)
    {
        _emailHandler = emailHandler;
        _userManager = userManager;
    }

    [HttpGet(ApiRoutes.Account.Password.Recovery)]
    [SwaggerOperation(Tags = new[] { "Account Endpoint" })]
    public override async Task<ActionResult> HandleAsync([FromQuery] PasswordRecoveryDto dto, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
            return BadRequest(new ErrorResponse
            {
                Errors = new[] { "Couldn't find user associated with this email" }
            });

        var emailHandled = await _emailHandler.SendPasswordRecoveryEmailAsync(user, ct);

        return emailHandled
            ? Ok()
            : StatusCode(500);
    }
}