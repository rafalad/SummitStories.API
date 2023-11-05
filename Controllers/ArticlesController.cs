using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SummitStories.API.Models;
using SummitStories.API.Modules.Data.Interfaces;
using SummitStories.API.Modules.Data.Models;

namespace SummitStories.API.Controllers;

[Authorize]
[ApiController]
[Route("api")]
[Produces("application/json")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleRepository _countryRepository;

    public ArticlesController(IArticleRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    [HttpOptions("articles")]
    public IActionResult Preflight()
    {
        return NoContent();
    }

    [HttpGet("articles")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<Article>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    public IActionResult GetCountries()
    {
        IList<Article> results = _countryRepository.GetCountries();
        return Ok(results);
    }
}
