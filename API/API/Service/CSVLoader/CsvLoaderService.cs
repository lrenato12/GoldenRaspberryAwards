using API.Models;
using API.Utils;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;

namespace API.Service.CSVLoader;

public class CsvLoaderService : ICsvLoaderService
{
    public List<MovieModel> LoadMovies(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = true,
        };

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);

        csv.Context.RegisterClassMap<MovieMap>();
        return csv.GetRecords<MovieModel>().ToList();
    }
}