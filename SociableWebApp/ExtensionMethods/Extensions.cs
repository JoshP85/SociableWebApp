namespace SociableWebApp.ExtensionMethods
{
    public static class Extensions
    {
        public static string GetTimeSince(this string datetime, DateTime dateTime)
        {
            TimeSpan timeDiff = dateTime - datetime.ConvertStringToDateTime();

            if (timeDiff.TotalDays >= 1) return $"{timeDiff.TotalDays:F0} days ago";

            if (timeDiff.TotalHours >= 1 && timeDiff.TotalHours < 24) return $"{timeDiff.TotalHours:F0} hours ago";

            if (timeDiff.TotalMinutes >= 1 && timeDiff.TotalMinutes < 60) return $"{timeDiff.TotalMinutes:F0} mins ago";

            if (timeDiff.TotalSeconds >= 1 && timeDiff.TotalSeconds < 60) return $"{timeDiff.TotalSeconds:F0} secs ago";

            return "Just now";
        }

        public static DateTime ConvertStringToDateTime(this string dateTime)
        {
            return DateTime.Parse(dateTime);
        }
    }
}
