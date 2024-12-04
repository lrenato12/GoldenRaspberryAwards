using API.Service.CSVLoader;
using API.Service.MovieService;

namespace API.Service.MovieDataImporter;

public class MovieDataImporter
{
    private readonly ICsvLoaderService _csvLoaderService;
    private readonly IMovieService _movieService;

    public MovieDataImporter(ICsvLoaderService csvLoaderService, IMovieService movieService)
    {
        _csvLoaderService = csvLoaderService;
        _movieService = movieService;
    }

    public void Import(string filePath)
    {
        var movies = _csvLoaderService.LoadMovies(filePath);
        _movieService.SaveMovies(movies);
    }
}