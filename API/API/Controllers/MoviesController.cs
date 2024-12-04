using API.Models;
using API.Service;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    #region [ PROPERTS ]
    private readonly MovieContext _context;

    private readonly ProducerIntervalService _producerIntervalService;
    #endregion

    #region [ CTOR ]
    public MoviesController(MovieContext context, ProducerIntervalService producerIntervalService)
    {
        _context = context;
        _producerIntervalService = producerIntervalService;
    }
    #endregion

    #region [ METHODS ]
    #region [ GET MOVIES ]
    /// <summary>
    /// Endpoint para retornar todos os registros
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetMovies")]
    public IActionResult GetMovies()
        => Ok(_context.Movies.ToList());
    #endregion

    #region [ GET PRODUCERS INTERVAL ]
    /// <summary>
    /// Endpoint para obter o produtor com maior e menor intervalo entre prêmios
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetProducersInterval")]
    public IActionResult GetProducersInterval()
        => Ok(_producerIntervalService.GetProducerIntervals());
    #endregion
    #endregion
}