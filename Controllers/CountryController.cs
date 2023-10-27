using Microsoft.AspNetCore.Mvc;
using SummitStories.API.Models;
using SummitStories.API.Modules.Data.Interfaces;
using SummitStories.API.Modules.Data.Models;

namespace SummitStories.API.Controllers;

[ApiController]
[Route("api")]
[Produces("application/json")]
public class CountryController : ControllerBase
{
    private readonly ICountryRepository _countryRepository;

    public CountryController(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    [HttpOptions("countries")]
    public IActionResult Preflight()
    {
        return NoContent();
    }

    [HttpGet("countries")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<Country>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    public IActionResult GetCountries()
    {
        IList<Country> results = _countryRepository.GetCountries();
        return Ok(results);
    }
}
