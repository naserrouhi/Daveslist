using Daveslist.Application.Users.AppServices.Interfaces;
using Daveslist.Application.Users.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Daveslist.Api.Controllers;

[ApiController]
[Route("api/roles")]
public class RolesController : ControllerBase
{
    private readonly IRoleAppService _roleAppService;

    public RolesController(IRoleAppService roleAppService)
    {
        _roleAppService = roleAppService;
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignRoleToUserAsync([FromBody] AssignRoleDto assignRoleDto)
    {
        await _roleAppService.AssignRoleToUserAsync(assignRoleDto);

        return Accepted();
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<IEnumerable<string>>> GetUserRoles(string username)
    {
        var roles = await _roleAppService.GetUserRolesAsync(username);

        return Ok(roles);
    }
}
