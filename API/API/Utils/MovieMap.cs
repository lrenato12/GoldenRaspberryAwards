using API.Models;
using CsvHelper.Configuration;

namespace API.Utils;

/// <summary>
/// Movie Map
/// </summary>
public class MovieMap : ClassMap<MovieModel>
{
    public MovieMap()
    {
        Map(m => m.year).Name("year");
        Map(m => m.title).Name("title");
        Map(m => m.studios).Name("studios");
        Map(m => m.producers).Name("producers");
        Map(m => m.winner).Name("winner").TypeConverter<BooleanYesNoConverter>();
    }
}