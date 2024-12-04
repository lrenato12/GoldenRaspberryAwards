using API.Models;

namespace API.Service.CSVLoader;

public interface ICsvLoaderService
{
    List<MovieModel> LoadMovies(string filePath);
}