using Microsoft.EntityFrameworkCore;

namespace API.Models;

/// <summary>
/// Movie Context
/// </summary>
public class MovieContext : DbContext
{
    #region [ PROPERTY ]
    public DbSet<MovieModel> Movies { get; set; }
    #endregion

    #region [ CTOR ]
    public MovieContext(DbContextOptions<MovieContext> options) : base(options) { }
    #endregion

    #region [ ON MODEL CREATING ]
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MovieModel>()
            .HasKey(m => m.title);
    }
    #endregion

    #region [ GET PRODUCERS INTERVAL ]
    /// <summary>
    /// Get Producers Interval
    /// </summary>
    /// <returns></returns>
    public (List<ProducerInterval> MaxInterval, List<ProducerInterval> MinInterval) GetProducersInterval()
    {
        var winners = this.Movies.Where(m => m.winner.Equals(true)).ToList();

        var groupedByProducer = winners.GroupBy(m => m.producers).Where(g => g.Count() > 1).ToList();

        var maxIntervalList = new List<ProducerInterval>();
        var minIntervalList = new List<ProducerInterval>();

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
    #endregion
}