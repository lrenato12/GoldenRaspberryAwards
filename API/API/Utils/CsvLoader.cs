using API.Models;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;

namespace API.Utils
{
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

            var movies = csv.GetRecords<Movie>().ToList();

            context.Movies.AddRange(movies);
            context.SaveChanges();
        }
    }
}