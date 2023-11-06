using System.Globalization;
using System.Text.RegularExpressions;
using Locompro.Models;

namespace Locompro.Common;

public static class DateFormatter
{
    /// <summary>
    /// Extracts from entry time, the date in the format yyyy-mm-dd
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string GetFormattedDateFromDateTime(DateTime dateTime)
    {
        // Define a timeout duration for the regex operation
        TimeSpan matchTimeout = TimeSpan.FromSeconds(2); // 1 second timeout

        // Use the Regex constructor that allows a timeout
        Regex regex = new Regex(@"[0-9]*/[0-9.]*/[0-9]*", RegexOptions.None, matchTimeout);

        // Perform the match with the timeout
        Match regexMatch = regex.Match(dateTime.ToString(CultureInfo.InvariantCulture));

        string date = regexMatch.Success
            ? regexMatch.Groups[0].Value
            : dateTime.ToString(CultureInfo.InvariantCulture);

        return date;
    }
    
    /// <summary>
    /// Gets a DateTime object from a string in the format yyyy-mm-dd
    /// </summary>
    /// <param name="formatedDateTime"></param>
    /// <returns></returns>
    public static DateTime GetDateTimeFromString(string formatedDateTime)
    {
        DateTime dateTime = DateTime.Parse(formatedDateTime, CultureInfo.InvariantCulture);
        
        return dateTime;
    }
}