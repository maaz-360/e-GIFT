namespace eGift.Admin.Helpers;

public static class DateTimeHelper
{
    #region DateTime to DateString
    public static string ToDateString(this DateTime date)
    {
        return date.ToString("yyyy-MM-dd");
    }

    public static string ToDateString(this DateTime? date)
    {
        return date?.ToString("yyyy-MM-dd") ?? string.Empty;
    }
    #endregion

    #region DateTime to DateTimeString
    public static string ToDateTimeString(this DateTime date)
    {
        return date.ToString("yyyy-MM-dd hh:mm:ss tt");
    }

    public static string ToDateTimeString(this DateTime? date)
    {
        return date?.ToString("yyyy-MM-dd hh:mm:ss tt") ?? string.Empty;
    }
    #endregion
}