using Daveslist.Application.PrivateMessages.AppServices;
using Daveslist.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Daveslist.Api.Controllers;

[ApiController]
[Route("api/private-messages")]
public class PrivateMessageController : ControllerBase
{
    private readonly IPrivateMessageAppService _privateMessageAppService;

    public PrivateMessageController(IPrivateMessageAppService privateMessageAppService)
    {
        _privateMessageAppService = privateMessageAppService;
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.User)]
    public async Task<IActionResult> GetListAsync(CancellationToken cancellationToken)
    {
        var messages = await _privateMessageAppService.GetListAsync(cancellationToken);

        return Ok(messages);
    }

    [HttpPost("users/{id:int}")]
    [Authorize(Roles = UserRoles.User)]
    public async Task<IActionResult> SendAsync([FromRoute] int id, [FromBody] string content, CancellationToken cancellationToken)
    {
        await _privateMessageAppService.SendAsync(id, content, cancellationToken);

        return Accepted();
    }
}
