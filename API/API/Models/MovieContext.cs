using Microsoft.EntityFrameworkCore;

namespace API.Models;

public class MovieContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }

    public MovieContext(DbContextOptions<MovieContext> options) : base(options) { }
}