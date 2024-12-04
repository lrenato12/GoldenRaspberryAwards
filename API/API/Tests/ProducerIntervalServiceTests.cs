using API.Models;
using API.Service;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class ProducerIntervalServiceTests
{
    private MovieContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<MovieContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        return new MovieContext(options);
    }

    [Fact]
    public void GetProducerIntervals_ShouldReturnCorrectIntervals()
    {
        using var context = GetInMemoryDbContext();

        context.Movies.AddRange(new List<MovieModel>
        {
            new MovieModel { year = 2008, producers = "Producer 1", winner = true },
            new MovieModel { year = 2009, producers = "Producer 1", winner = true },
            new MovieModel { year = 2018, producers = "Producer 2", winner = true },
            new MovieModel { year = 2019, producers = "Producer 2", winner = true },
            new MovieModel { year = 2000, producers = "Producer 3", winner = true },
            new MovieModel { year = 2010, producers = "Producer 3", winner = true }
        });

        context.SaveChanges();

        var service = new ProducerIntervalService(context);

        var result = service.GetProducerIntervals();

        Assert.NotNull(result);
        Assert.NotEmpty(result.Min);
        Assert.NotEmpty(result.Max);

        Assert.Equal(1, result.Min.First().Interval);
        Assert.Equal("Producer 1", result.Min.First().Producer);
        Assert.Equal(2008, result.Min.First().PreviousWin);
        Assert.Equal(2009, result.Min.First().FollowingWin);

        Assert.Equal(10, result.Max.First().Interval);
        Assert.Equal("Producer 3", result.Max.First().Producer);
        Assert.Equal(2000, result.Max.First().PreviousWin);
        Assert.Equal(2010, result.Max.First().FollowingWin);
    }
}