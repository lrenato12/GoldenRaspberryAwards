using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("[controller]")]
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
    public IActionResult GetMovies() => Ok(_context.Movies.ToList());
}