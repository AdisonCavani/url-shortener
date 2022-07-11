using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UrlShortener.Core;
using UrlShortener.Core.Contracts.V1;
using UrlShortener.Core.Models.Requests;
using UrlShortener.WebApi.Models.Entities;
using UrlShortener.WebApi.Services;
using UrlShortener.WebApi.Services.Auth;

namespace UrlShortener.WebApi.Endpoints.Account.Password;

public class VerifyToken : EndpointBaseAsync.WithRequest<PasswordRecoveryTokenDto>.WithActionResult
{
    private readonly UserManager<AppUser> _userManager;

    public VerifyToken(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost(ApiRoutes.Account.Password.VerifyToken)]
    [SwaggerOperation(Tags = new[] { "Account Endpoint" })]
    public override async Task<ActionResult> HandleAsync([FromBody] PasswordRecoveryTokenDto dto, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
            return BadRequest(new ErrorResponse
            {
                Errors = new[] { "Couldn't find user associated with this email" }
            });

        if (!await _userManager.VerifyUserTokenAsync(user, PasswordResetTokenProvider.ProviderKey,
                PasswordResetTokenProvider.ProviderKey, dto.Token))
            return BadRequest(new ErrorResponse
            {
                Errors = new[] { "Invalid token" }
            });

        return Ok();
    }
}