using Microsoft.AspNetCore.Mvc;
using UrlShortener.Contracts.V2;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers.V2;

[ApiController]
[ApiVersion("2")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost(ApiRoutes.Account.Register)]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto dto)
    {
        await _accountService.RegisterUser(dto);
        return Ok();
    }
}