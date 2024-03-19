namespace WebApp.Utilities;

public static class ViewHelperUtilities
{
    public static string Truncate(string value, int length)
    {
        if (value.Length <= length)
        {
            return value;
        }
        return value.Substring(0, length) + "...";
    }
    public static string ToTimeAgo(DateTime time)
    {
        TimeSpan timeSinceCreation = DateTime.Now - time;

        if (timeSinceCreation.TotalSeconds < 60)
        {
            int seconds = (int)timeSinceCreation.TotalSeconds;
            return $"{seconds} second{(seconds != 1 ? "s" : "")} ago";
        }

        if (timeSinceCreation.TotalMinutes < 60)
        {
            int minutes = (int)timeSinceCreation.TotalMinutes;
            return $"{minutes} minute{(minutes != 1 ? "s" : "")} ago";
        }

        if (timeSinceCreation.TotalHours < 24)
        {
            int hours = (int)timeSinceCreation.TotalHours;
            return $"{hours} hour{(hours != 1 ? "s" : "")} ago";
        }

        return time.ToString("yyyy-MM-dd");
    }
}

