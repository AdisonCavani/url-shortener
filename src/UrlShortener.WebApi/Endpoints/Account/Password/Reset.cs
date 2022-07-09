using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UrlShortener.Core;
using UrlShortener.Core.Contracts.V1;
using UrlShortener.Core.Models.Requests;
using UrlShortener.WebApi.Models.Entities;
using UrlShortener.WebApi.Services;

namespace UrlShortener.WebApi.Endpoints.Account.Password;

public class Reset : EndpointBaseAsync.WithRequest<ResetPasswordDto>.WithActionResult
{
    private readonly EmailHandler _emailHandler;
    private readonly UserManager<AppUser> _userManager;

    public Reset(EmailHandler emailHandler, UserManager<AppUser> userManager)
    {
        _emailHandler = emailHandler;
        _userManager = userManager;
    }

    [HttpPost(ApiRoutes.Account.Password.Reset)]
    [SwaggerOperation(Tags = new[] { "Account Endpoint" })]
    public override async Task<ActionResult> HandleAsync([FromBody] ResetPasswordDto dto, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
            return StatusCode(500);

        var result =
            await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);

        if (!result.Succeeded)
            return BadRequest(new ErrorResponse
            {
                Errors = result.Errors.Select(x => x.Description)
            });

        var emailHandled = await _emailHandler.SendPasswordChangedAlertAsync(user, ct);

        return emailHandled
            ? Ok()
            : StatusCode(500);
    }
}