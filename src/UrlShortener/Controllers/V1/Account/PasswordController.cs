using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using UrlShortener.Core;
using UrlShortener.Core.Contracts.V1;
using UrlShortener.Core.Models.Requests;
using UrlShortener.Models.Entities;
using UrlShortener.Services;

namespace UrlShortener.Controllers.V1.Account;

[ApiController]
public class PasswordController : ControllerBase
{
    private readonly EmailHandler _emailHandler;
    private readonly UserManager<AppUser> _userManager;

    public PasswordController(EmailHandler emailHandler, UserManager<AppUser> userManager)
    {
        _emailHandler = emailHandler;
        _userManager = userManager;
    }

    [HttpGet(ApiRoutes.Account.Password.SendRecoveryEmail)]
    public async Task<IActionResult> SendRecoveryEmailAsync([FromQuery] PasswordRecoveryDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
            return BadRequest(new ErrorResponse
            {
                Errors = new[] { "Couldn't find user associated with this email" }
            });

        var emailHandled = await _emailHandler.SendPasswordRecoveryEmailAsync(user);

        return emailHandled
            ? Ok()
            : StatusCode(500);
    }

    [HttpPost(ApiRoutes.Account.Password.VerifyToken)]
    public async Task<IActionResult> VerifyTokenAsync([FromBody] PasswordRecoveryTokenDto dto)
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

    [HttpPost(ApiRoutes.Account.Password.Reset)]
    public async Task<IActionResult> ResetAsync([FromBody] ResetPasswordDto dto, CancellationToken token)
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

        var emailHandled = await _emailHandler.SendPasswordChangedAlertAsync(user, token);

        return emailHandled
            ? Ok()
            : StatusCode(500);
    }

    [Authorize]
    [HttpPost(ApiRoutes.Account.Password.Change)]
    public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDto dto, CancellationToken token)
    {
        var uid = HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (string.IsNullOrEmpty(uid))
            return StatusCode(500);

        var user = await _userManager.FindByIdAsync(uid);

        if (user is null)
            return StatusCode(500);

        var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword,
            dto.NewPassword); // TODO: password might be the same!

        if (!result.Succeeded)
            return BadRequest(new ErrorResponse
            {
                Errors = result.Errors.Select(x => x.Description)
            });

        var emailHandled = await _emailHandler.SendPasswordChangedAlertAsync(user, token);

        return emailHandled
            ? Ok()
            : StatusCode(500);
    }
}