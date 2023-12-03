namespace WpfApplication.Helpers;

public static class Utils
{
    public static bool CheckTwoDatesOnEqualityByDays(DateTime one, DateTime second)
    {
        return one.Day == second.Day && one.Month == second.Month && one.Year == second.Year;
    }

    public static IEnumerable<string> GetDayNamesStartingMonday()
    {
        return CultureInfo.CurrentCulture.DateTimeFormat.DayNames.Skip(1).Concat(
            CultureInfo.CurrentCulture.DateTimeFormat.DayNames.Take(1));
    }
}