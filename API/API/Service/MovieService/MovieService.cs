using API.Models;

namespace API.Service.MovieService;

public class MovieService : IMovieService
{
    private readonly MovieContext _context;

    public MovieService(MovieContext context)
    {
        _context = context;
    }

    public void SaveMovies(List<MovieModel> movies)
    {
        if (movies == null || !movies.Any())
            throw new ArgumentException("No movies to save.");

        var all = _context.Movies.ToList();
        _context.Movies.RemoveRange(all);
        _context.SaveChanges();

        _context.Movies.AddRange(movies);
        _context.SaveChanges();
    }
}