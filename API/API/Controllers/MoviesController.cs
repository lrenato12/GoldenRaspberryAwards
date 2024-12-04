using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly MovieContext _context;

    public MoviesController(MovieContext context) { _context = context; }

    /// <summary>
    /// Get Movies
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetMovies")]
    public IActionResult GetMovies()
    {
        var movies = _context.Movies.ToList();
        return Ok(movies);
    }

    /// <summary>
    /// Endpoint para obter o produtor com maior e menor intervalo entre prêmios
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetProducersInterval")]
    public IActionResult GetProducersInterval()
    {
        var result = _context.GetProducersInterval();
        return Ok(new
        {
            max = result.MaxInterval,
            min = result.MinInterval
        });
    }
}