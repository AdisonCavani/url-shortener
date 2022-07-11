using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UrlShortener.Core;
using UrlShortener.Core.Contracts.V1;
using UrlShortener.Core.Models.Requests;
using UrlShortener.WebApi.Models.Entities;
using UrlShortener.WebApi.Services.Interfaces;

namespace UrlShortener.WebApi.Endpoints.Account;

public class Register : EndpointBaseAsync.WithRequest<RegisterCredentialsDto>.WithActionResult
{
    private readonly IEmailHandler _emailHandler;
    private readonly UserManager<AppUser> _userManager;

    public Register(IEmailHandler emailHandler, UserManager<AppUser> userManager)
    {
        _emailHandler = emailHandler;
        _userManager = userManager;
    }

    [HttpPost(ApiRoutes.Account.Register)]
    [SwaggerOperation(Tags = new[] { "Account Endpoint" })]
    public override async Task<ActionResult> HandleAsync([FromBody] RegisterCredentialsDto dto,
        CancellationToken ct = default)
    {
        AppUser user = new()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            UserName = dto.Email // TODO: Resolve this duplication
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            return BadRequest(new ErrorResponse
            {
                Errors = result.Errors.Select(x => x.Description)
            });

        var createdUser = await _userManager.FindByEmailAsync(dto.Email);

        if (createdUser is null)
            return StatusCode(500);

        var emailHandled = await _emailHandler.SendVerificationEmailAsync(user, ct);

        return emailHandled
            ? Ok()
            : StatusCode(500);
    }
}