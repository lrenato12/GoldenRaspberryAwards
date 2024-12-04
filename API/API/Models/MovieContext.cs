using Microsoft.EntityFrameworkCore;

namespace API.Models;

public class MovieContext : DbContext
{
    public DbSet<MovieModel> Movies { get; set; }

    public MovieContext(DbContextOptions<MovieContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MovieModel>()
            .HasKey(m => m.title);
    }

    public (List<ProducerInterval> MaxInterval, List<ProducerInterval> MinInterval) GetProducersInterval()
    {
        // Filtra os filmes vencedores
        //var winners = this.Movies.Where(m => m.winner.HasValue && m.winner.Equals(true)).ToList();
        var winners = this.Movies.Where(m => m.winner.Equals(true)).ToList();

        // Agrupa os filmes por produtor e ordena por ano
        var groupedByProducer = winners
            .GroupBy(m => m.producers)
            .Where(g => g.Count() > 1) // Considera apenas produtores com mais de um prêmio
            .ToList();

        var maxIntervalList = new List<ProducerInterval>();
        var minIntervalList = new List<ProducerInterval>();

        // Calcula os intervalos entre os prêmios consecutivos para cada produtor
        foreach (var producerGroup in groupedByProducer)
        {
            var sortedByYear = producerGroup.OrderBy(m => m.year).ToList();

            for (int i = 1; i < sortedByYear.Count(); i++)
            {
                var previousWin = sortedByYear[i - 1];
                var followingWin = sortedByYear[i];

                var interval = followingWin.year - previousWin.year;

                if (maxIntervalList.Count == 0 || interval > maxIntervalList.First().Interval)
                {
                    maxIntervalList.Clear();
                    maxIntervalList.Add(new ProducerInterval
                    {
                        Producer = producerGroup.Key,
                        Interval = interval,
                        PreviousWin = previousWin.year,
                        FollowingWin = followingWin.year
                    });
                }

                if (minIntervalList.Count == 0 || interval < minIntervalList.First().Interval)
                {
                    minIntervalList.Clear();
                    minIntervalList.Add(new ProducerInterval
                    {
                        Producer = producerGroup.Key,
                        Interval = interval,
                        PreviousWin = previousWin.year,
                        FollowingWin = followingWin.year
                    });
                }
            }
        }

        return (maxIntervalList, minIntervalList);
    }
}