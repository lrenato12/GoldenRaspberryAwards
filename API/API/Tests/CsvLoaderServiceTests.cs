using API.Service.CSVLoader;
using Xunit;

namespace API.Tests;

public class CsvLoaderServiceTests
{
    [Fact]
    public void LoadMovies_ValidFile_ShouldReturnMovies()
    {
        var service = new CsvLoaderService();
        var filePath = Path.Combine(AppContext.BaseDirectory, "movies.csv");

        var movies = service.LoadMovies(filePath);

        Assert.NotNull(movies);
        Assert.True(movies.Count > 0);
    }
}