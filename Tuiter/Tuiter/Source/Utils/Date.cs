namespace Tuiter.Source.Utils
{
    using System;

    internal static class Date
    {
        internal static string TimeElapsed(DateTime date)
        {
            string span;
            var difference = DateTime.Now - date;

            if (difference.TotalDays > 1)
                span = string.Format("{0}d ago", Math.Round(difference.TotalDays));
            else if (difference.TotalHours > 1)
                span = string.Format("{0}h ago", Math.Round(difference.TotalHours));
            else if (difference.TotalMinutes > 1)
                span = string.Format("{0}m ago", Math.Round(difference.TotalMinutes));
            else
                span = "Just now!";

            return span;
        }

        internal static DateTime ToDate(string date)
        {
            var splitDate = date.Split(' ');
            var dateTime = DateTime.Parse(splitDate[0] + ", " + splitDate[1]
                                          + " " + splitDate[2] + " " + splitDate[5]
                                          + " " + splitDate[3]);
            return dateTime;
        }
    }
}