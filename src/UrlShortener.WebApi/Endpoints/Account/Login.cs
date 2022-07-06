using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UrlShortener.Core.Contracts.V1;
using UrlShortener.Core.Models.Requests;
using UrlShortener.Core.Models.Responses;
using UrlShortener.WebApi.Models.Entities;
using UrlShortener.WebApi.Services;

namespace UrlShortener.WebApi.Endpoints.Account;

public class Login : EndpointBaseAsync.WithRequest<LoginCredentialsDto>.WithActionResult
{
    private readonly JwtService _jwtService;
    private readonly SignInManager<AppUser> _signInManager;

    public Login(JwtService jwtService, SignInManager<AppUser> signInManager)
    {
        _jwtService = jwtService;
        _signInManager = signInManager;
    }

    [HttpPost(ApiRoutes.Account.Login)]
    [SwaggerOperation(Tags = new[] {"Account Endpoint"})]
    public override async Task<ActionResult> HandleAsync([FromBody] LoginCredentialsDto dto,
        CancellationToken ct = default)
    {
        var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, true, true);

        if (result.IsNotAllowed)
            return Conflict();

        if (result.IsLockedOut)
            return StatusCode(StatusCodes.Status403Forbidden);

        if (!result.Succeeded)
            return BadRequest();

        var user = await _signInManager.UserManager.FindByEmailAsync(dto.Email);

        if (user is null)
            return StatusCode(500);

        var token = _jwtService.GenerateToken(user);

        return Ok(new JwtTokenDto
        {
            Token = token.Token
        });
    }
}