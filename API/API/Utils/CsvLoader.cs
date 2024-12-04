using API.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace API.Utils;

public class CsvLoader
{
    #region [ LOAD CSV DATA ]
    /// <summary>
    /// Metodo responsavel por carregar os itens do CSV.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="filePath"></param>
    /// <exception cref="FileNotFoundException"></exception>
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

        csv.Context.RegisterClassMap<MovieMap>();
        var movies = csv.GetRecords<MovieModel>().ToList();

        var all = from c in context.Movies select c;
        context.Movies.RemoveRange(all);
        context.SaveChanges();

        context.Movies.AddRange(movies);
        context.SaveChanges();
    } 
    #endregion
}