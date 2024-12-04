using CsvHelper.Configuration;

namespace API.Models.Map;

public class MovieMap : ClassMap<MovieModel>
{
    public MovieMap()
    {
        //Map(m => m.Id).Name("Id");
        Map(m => m.year).Name("title");
        Map(m => m.producers).Name("producers");
        Map(m => m.studios).Name("studios");
        Map(m => m.year).Name("year");
        Map(m => m.winner).Name("winner");
    }
}