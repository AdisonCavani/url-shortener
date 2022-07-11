using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using UrlShortener.Core;
using UrlShortener.Core.Contracts.V1;
using UrlShortener.Core.Models.Requests;
using UrlShortener.WebApi.Models.Entities;
using UrlShortener.WebApi.Services.Interfaces;

namespace UrlShortener.WebApi.Endpoints.Account.Password;

public class Change : EndpointBaseAsync.WithRequest<ChangePasswordDto>.WithActionResult
{
    private readonly IEmailHandler _emailHandler;
    private readonly UserManager<AppUser> _userManager;

    public Change(IEmailHandler emailHandler, UserManager<AppUser> userManager)
    {
        _emailHandler = emailHandler;
        _userManager = userManager;
    }

    [Authorize]
    [HttpPost(ApiRoutes.Account.Password.Change)]
    [SwaggerOperation(Tags = new[] { "Account Endpoint" })]
    public override async Task<ActionResult> HandleAsync([FromBody] ChangePasswordDto dto, CancellationToken ct = default)
    {
        var uid = HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (string.IsNullOrEmpty(uid))
            return StatusCode(500);

        var user = await _userManager.FindByIdAsync(uid);

        if (user is null)
            return StatusCode(500);

        var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

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