using API.Models;

namespace API.Service.MovieService;

public interface IMovieService
{
    void SaveMovies(List<MovieModel> movies);
}