using Daveslist.Application.Users.AppServices.Interfaces;
using Daveslist.Application.Users.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Daveslist.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserAppService _userAppService;

    public UserController(IUserAppService userAppService)
    {
        _userAppService = userAppService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto registerDto)
    {
        await _userAppService.RegisterAsync(registerDto);

        return Accepted();
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
    {
        await _userAppService.LoginAsync(loginDto);

        return Ok();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        await _userAppService.LogoutAsync();

        return Accepted();
    }
}
