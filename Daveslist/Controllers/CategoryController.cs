using Daveslist.Application.Categories.AppServices;
using Daveslist.Application.Categories.Models;
using Microsoft.AspNetCore.Mvc;

namespace Daveslist.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryAppService _categoryAppService;

    public CategoryController(ICategoryAppService categoryAppService)
    {
        _categoryAppService = categoryAppService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetListAsync(CancellationToken cancellationToken)
    {
        var categories = await _categoryAppService.GetListAsync(cancellationToken);

        return Ok(categories);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateAsync([FromQuery] string name, [FromQuery] bool isPublic, CancellationToken cancellationToken)
    {
        var category = await _categoryAppService.CreateAsync(name, isPublic, cancellationToken);

        return Ok(category);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _categoryAppService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }
}
