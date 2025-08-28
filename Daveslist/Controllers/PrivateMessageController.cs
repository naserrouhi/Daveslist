using Daveslist.Application.PrivateMessages.AppServices;
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
    public async Task<IActionResult> GetListAsync(CancellationToken cancellationToken)
    {
        var messages = await _privateMessageAppService.GetListAsync(cancellationToken);

        return Ok(messages);
    }

    [HttpPost("users/{userId:int}")]
    public async Task<IActionResult> SendAsync([FromRoute] int userId, [FromBody] string content, CancellationToken cancellationToken)
    {
        await _privateMessageAppService.SendAsync(userId, content, cancellationToken);

        return Accepted();
    }
}
