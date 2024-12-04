using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace API.Utils;

public class BooleanYesNoConverter : BooleanConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        try
        {
            if (string.Equals(text, "yes", StringComparison.OrdinalIgnoreCase))
                return true;
            if (string.IsNullOrEmpty(text) || string.Equals(text, "no", StringComparison.OrdinalIgnoreCase))
                return false;

            return base.ConvertFromString(text, row, memberMapData);
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}