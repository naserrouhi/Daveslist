using Daveslist.Application.Listings.AppServices;
using Daveslist.Application.Listings.Models;
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ListingDto>>> GetListAsync(CancellationToken cancellationToken)
    {
        var listings = await _listingAppService.GetListAsync(1, cancellationToken);

        return Ok(listings);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ListingDto>> GetAsync(int id, CancellationToken cancellationToken)
    {
        var listing = await _listingAppService.GetAsync(id, cancellationToken);

        return Ok(listing);
    }

    [HttpPost]
    public async Task<ActionResult<ListingDto>> CreateAsync([FromBody] UpsertListingDto upsertListingDto, CancellationToken cancellationToken)
    {
        var listing = await _listingAppService.CreateAsync(upsertListingDto, cancellationToken);

        return Ok(listing);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpsertListingDto upsertListingDto, CancellationToken cancellationToken)
    {
        var listing = await _listingAppService.UpdateAsync(id, upsertListingDto, cancellationToken);

        return Ok(listing);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _listingAppService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpPost("{id:int}:hide")]
    public async Task<IActionResult> HideAsync(int id, CancellationToken cancellationToken)
    {
        await _listingAppService.HideAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpPost("{id:int}:unhide")]
    public async Task<IActionResult> UnhideAsync(int id, CancellationToken cancellationToken)
    {
        await _listingAppService.UnhideAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpGet("{id:int}/replies")]
    public async Task<ActionResult<IEnumerable<ReplyDto>>> GetReplies(int id, CancellationToken cancellationToken)
    {
        var replies = await _listingAppService.GetRepliesAsync(id, cancellationToken);

        return Ok(replies);
    }

    [HttpPost("{id:int}/replies")]
    public async Task<ActionResult<ListingDto>> ReplyAsync([FromRoute] int id, [FromBody] string content, CancellationToken cancellationToken)
    {
        var listing = await _listingAppService.ReplyAsync(id, content, cancellationToken);

        return Ok(listing);
    }
}
