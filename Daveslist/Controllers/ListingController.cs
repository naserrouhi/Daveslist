using Daveslist.Application.Listings.AppServices;
using Daveslist.Application.Listings.Models;
using Daveslist.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Daveslist.Api.Controllers;

[ApiController]
[Route("api/listings")]
public class ListingController : ControllerBase
{
    private readonly IListingAppService _listingAppService;

    public ListingController(IListingAppService listingAppService)
    {
        _listingAppService = listingAppService;
    }

    [HttpGet("categories/{id:int}")]
    public async Task<ActionResult<IEnumerable<ListingDto>>> GetListAsync(int id, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var isUserAuthenticated = !User.Identity?.IsAuthenticated ?? false;

        var listings = await _listingAppService.GetListAsync(isUserAuthenticated, id, pageNumber, pageSize, cancellationToken);

        return Ok(listings);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ListingDto>> GetAsync(int id, CancellationToken cancellationToken)
    {
        var isUserAuthenticated = !User.Identity?.IsAuthenticated ?? false;

        var listing = await _listingAppService.GetAsync(isUserAuthenticated, id, cancellationToken);

        return Ok(listing);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.User)]
    public async Task<ActionResult<ListingDto>> CreateAsync([FromBody] UpsertListingDto upsertListingDto, CancellationToken cancellationToken)
    {
        var listing = await _listingAppService.CreateAsync(upsertListingDto, cancellationToken);

        return Ok(listing);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = $"{UserRoles.User},{UserRoles.Admin}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpsertListingDto upsertListingDto, CancellationToken cancellationToken)
    {
        var listing = await _listingAppService.UpdateAsync(id, upsertListingDto, isAdmin: User.IsInRole(UserRoles.Admin), cancellationToken);

        return Ok(listing);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = $"{UserRoles.User},{UserRoles.Admin}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _listingAppService.DeleteAsync(id, isAdmin: User.IsInRole(UserRoles.Admin), cancellationToken);

        return NoContent();
    }

    [HttpPost("{id:int}:hide")]
    [Authorize(Roles = $"{UserRoles.Moderator},{UserRoles.Admin}")]
    public async Task<IActionResult> HideAsync(int id, CancellationToken cancellationToken)
    {
        await _listingAppService.HideAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpPost("{id:int}:unhide")]
    [Authorize(Roles = $"{UserRoles.Moderator},{UserRoles.Admin}")]
    public async Task<IActionResult> UnhideAsync(int id, CancellationToken cancellationToken)
    {
        await _listingAppService.UnhideAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpPost("{id:int}/replies")]
    [Authorize(Roles = UserRoles.User)]
    public async Task<ActionResult<ListingDto>> ReplyAsync([FromRoute] int id, [FromBody] string content, CancellationToken cancellationToken)
    {
        var listing = await _listingAppService.ReplyAsync(id, content, cancellationToken);

        return Ok(listing);
    }
}
