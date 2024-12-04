using API.Models;

namespace API.Service;

/// <summary>
/// Producer Interval Service
/// </summary>
public class ProducerIntervalService
{
    /// <summary>
    /// Property
    /// </summary>
    private readonly MovieContext _context;

    /// <summary>
    /// CTOR
    /// </summary>
    /// <param name="context"></param>
    public ProducerIntervalService(MovieContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get Producer Intervals
    /// </summary>
    /// <returns></returns>
    public ProducerIntervalResponse GetProducerIntervals()
    {
        var movies = _context.Movies
            .Where(m => m.winner.Equals(true))
            .ToList();

        var producerWins = movies
            .SelectMany(m => m.producers.Split(",").Select(p => new { Producer = p.Trim(), m.year }))
            .GroupBy(p => p.Producer)
            .Select(g => new
            {
                Producer = g.Key,
                Wins = g.Select(x => x.year).OrderBy(year => year).ToList()
            })
            .Where(g => g.Wins.Count > 1)
            .ToList();

        var intervals = producerWins
            .SelectMany(g =>
                g.Wins.Zip(g.Wins.Skip(1), (prev, next) => new ProducerInterval
                {
                    Producer = g.Producer,
                    Interval = next - prev,
                    PreviousWin = prev,
                    FollowingWin = next
                })
            )
            .ToList();

        var minInterval = intervals
            .Where(i => i.Interval == intervals.Min(x => x.Interval))
            .ToList();

        var maxInterval = intervals
            .Where(i => i.Interval == intervals.Max(x => x.Interval))
            .ToList();

        return new ProducerIntervalResponse
        {
            Min = minInterval,
            Max = maxInterval
        };
    }
}