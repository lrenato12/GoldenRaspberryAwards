using API.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace API.Utils;

public class CsvLoader
{
    public static void LoadCsvData(MovieContext context, string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException($"File not found: {filePath}");

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = true,
        };

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);

        var movies = csv.GetRecords<MovieModel>().ToList();

        var all = from c in context.Movies select c;
        context.Movies.RemoveRange(all);
        context.SaveChanges();

        context.Movies.AddRange(movies);
        context.SaveChanges();
    }
}