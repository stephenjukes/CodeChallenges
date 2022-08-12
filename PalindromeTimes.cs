using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NbsCodeChallenges
{
    public static class PalindromeTimes
    {
        public static void Run()
        {
            // var times = new string[] {"23:00:11", "23:00:21", "23:17:30", "23:58:30", "23:48:36", "23:25:50", "23:54:54", "23:29:01", "23:41:24", "23:05:06", "23:34:59", "23:18:58", "23:56:37", "23:51:56", "23:17:07", "23:02:04", "23:13:35", "23:05:04", "23:00:32", "23:57:30"};
            var times = new string[] { "13:42:37", "05:58:16", "04:43:32", "10:28:32", "17:52:00", "03:09:41", "21:01:06", "06:06:14", "10:44:03", "13:08:24", "12:19:28", "03:36:28", "08:07:54", "20:20:40", "01:23:20", "06:06:03", "08:04:45", "12:52:15", "07:27:32", "05:45:26", "22:06:36", "16:01:56", "04:13:09", "18:39:56", "18:08:12", "18:01:43", "15:57:22", "00:33:17", "13:20:49", "01:05:56", "00:09:50", "04:37:34", "21:07:14", "18:40:35", "19:38:42", "11:50:40", "08:08:48", "06:27:11", "06:45:48", "21:04:47", "02:37:33", "14:20:04", "10:40:29", "18:08:05", "04:02:18", "04:38:03", "03:20:41", "22:22:46", "02:58:19", "11:29:20", "21:30:39", "04:49:41", "17:43:00", "09:20:42", "22:46:13", "12:42:16", "07:22:25", "18:46:56", "12:08:11", "21:52:21", "19:01:04", "04:38:03", "20:39:48", "17:39:36", "16:27:15", "17:42:53", "10:55:13", "00:35:52", "22:08:33", "22:47:30", "10:19:19", "23:00:11", "15:57:02", "09:01:28", "02:49:24", "22:40:07", "22:22:07", "11:16:05", "11:37:36", "23:00:21", "05:07:23", "12:42:04", "09:59:37", "22:27:44", "01:28:33", "22:56:42", "10:25:25", "14:13:06", "15:01:22", "23:17:30", "23:58:30", "17:01:38", "21:40:09", "01:15:00", "00:15:30", "08:12:36", "14:35:43", "08:55:01", "18:37:35", "04:41:21", "10:53:50", "16:36:12", "00:37:42", "07:08:25", "22:36:10", "00:49:56", "17:09:17", "19:38:46", "13:22:42", "20:12:33", "07:27:56", "01:43:17", "04:45:22", "02:00:30", "10:08:49", "22:54:04", "19:33:05", "14:53:39", "02:35:20", "18:55:03", "19:46:56", "01:15:45", "06:17:12", "19:43:28", "18:39:04", "15:27:37", "12:59:29", "12:49:20", "12:31:02", "13:47:00", "01:28:28", "19:46:08", "08:39:06", "13:40:06", "17:30:50", "07:23:22", "05:50:56", "15:55:03", "15:37:17", "03:31:22", "07:17:15", "14:13:58", "19:03:33", "09:23:41", "23:48:36", "01:35:50", "11:42:51", "02:45:29", "04:00:11", "20:46:15", "11:30:17", "18:30:32", "04:32:48", "11:06:21", "21:26:55", "16:23:49", "06:51:48", "06:51:46", "14:31:29", "15:10:15", "12:19:21", "06:12:56", "17:04:24", "00:44:52", "14:21:23", "05:19:36", "19:20:19", "09:16:51", "11:19:45", "01:21:44", "01:43:30", "23:25:50", "15:22:07", "03:34:44", "17:32:02", "04:11:14", "02:38:49", "23:54:54", "10:54:18", "21:22:06", "10:56:24", "05:09:27", "13:32:33", "08:54:19", "08:35:10", "14:19:25", "16:24:22", "19:38:26", "22:23:58", "05:32:25", "21:10:15", "21:05:26", "13:32:05", "15:33:42", "20:14:24", "14:57:16", "13:44:05", "12:08:12", "10:10:08", "15:50:56", "00:56:38", "09:26:45", "07:13:31", "06:56:04", "20:59:52", "20:54:39", "18:52:42", "06:35:00", "08:38:37", "11:51:56", "04:09:35", "04:34:55", "15:04:34", "17:07:06", "04:27:57", "16:46:19", "07:34:57", "08:27:12", "09:20:09", "02:05:42", "11:09:35", "07:44:07", "06:53:52", "08:33:56", "08:37:11", "01:39:05", "17:42:51", "04:36:32", "07:12:43", "18:34:37", "07:44:43", "05:28:00", "07:06:18", "23:29:01", "17:57:21", "22:56:51", "01:05:14", "19:43:46", "18:22:58", "15:32:16", "18:47:47", "05:56:41", "01:21:51", "23:41:24", "03:46:26", "10:43:28", "01:06:27", "22:17:00", "01:46:47", "11:25:39", "06:16:01", "13:24:03", "07:49:56", "15:33:52", "03:11:09", "23:05:06", "03:22:52", "23:34:59", "04:41:35", "15:34:24", "10:07:18", "22:24:06", "04:20:08", "16:54:14", "02:06:22", "10:53:54", "20:40:43", "09:55:20", "04:27:30", "08:57:02", "20:05:44", "18:21:06", "13:19:12", "11:49:15", "23:18:58", "04:59:34", "17:07:29", "08:43:04", "20:35:48", "08:53:00", "11:01:17", "17:13:37", "16:05:13", "08:46:10", "19:48:26", "16:34:07", "10:13:52", "08:23:26", "12:37:01", "09:21:54", "00:18:35", "07:00:44", "07:01:09", "08:32:14", "02:15:21", "01:21:58", "17:06:57", "01:29:31", "06:37:12", "12:19:15", "07:04:48", "08:30:54", "03:12:52", "09:52:26", "19:05:46", "08:42:09", "17:58:08", "22:13:48", "11:14:47", "02:41:00", "10:54:21", "14:15:27", "09:13:49", "15:09:21", "13:56:06", "02:36:54", "14:38:13", "04:58:34", "07:56:55", "07:38:16", "02:13:07", "08:47:13", "22:37:22", "17:38:59", "03:42:35", "03:58:08", "06:46:44", "14:54:12", "01:10:06", "07:57:36", "14:20:30", "18:12:36", "23:56:37", "14:01:14", "06:19:17", "04:28:10", "21:28:19", "16:52:46", "19:40:01", "06:01:18", "01:08:22", "13:26:58", "05:14:58", "16:01:55", "02:35:47", "08:35:40", "17:32:55", "12:02:41", "13:05:27", "18:11:01", "09:29:34", "06:05:26", "05:26:34", "13:33:26", "19:07:17", "17:38:02", "20:06:45", "16:15:40", "00:51:58", "14:40:24", "12:33:19", "09:03:24", "20:51:10", "13:15:07", "21:05:46", "07:07:59", "05:49:48", "14:32:25", "01:18:12", "16:40:09", "00:16:33", "18:18:36", "19:53:28", "15:03:45", "22:37:17", "08:01:25", "01:53:49", "23:51:56", "22:15:25", "21:55:37", "19:10:08", "14:56:35", "22:48:09", "07:14:58", "03:45:20", "02:10:09", "17:59:52", "13:40:03", "23:17:07", "05:14:39", "09:51:09", "14:02:05", "06:34:44", "22:08:53", "19:33:43", "09:04:12", "21:07:10", "08:48:02", "15:07:44", "02:07:57", "14:11:11", "01:50:36", "22:25:43", "15:39:32", "19:57:04", "05:13:31", "11:19:11", "16:17:28", "03:40:19", "14:24:46", "03:04:28", "20:39:07", "19:17:36", "00:56:35", "10:58:00", "09:28:04", "11:12:27", "11:29:29", "09:59:20", "16:03:46", "06:08:10", "08:08:03", "16:35:32", "00:07:06", "04:38:14", "02:39:17", "17:48:41", "07:15:28", "09:47:55", "03:04:56", "14:26:45", "11:45:59", "18:03:14", "22:59:25", "23:02:04", "15:34:14", "04:34:27", "18:19:16", "05:23:47", "15:08:39", "14:00:47", "23:13:35", "00:46:45", "00:50:42", "00:04:03", "16:55:46", "05:49:22", "02:56:45", "16:51:40", "14:26:39", "03:06:40", "07:05:46", "03:19:04", "23:05:04", "06:46:57", "10:01:38", "15:31:29", "23:00:32", "20:43:25", "20:31:22", "12:45:21", "12:44:37", "12:30:12", "01:57:12", "10:39:07", "06:19:39", "01:49:04", "11:00:41", "06:09:47", "18:05:01", "12:47:36", "21:51:21", "11:20:04", "21:59:54", "21:24:21", "07:43:57", "04:03:25", "01:23:55", "19:29:24", "20:44:23", "15:00:55", "12:30:50", "12:21:09", "03:51:33", "01:21:12", "21:43:04", "16:20:10", "14:32:08", "09:14:02", "07:24:52", "23:57:30", "17:26:01", "16:54:01", "13:31:17", "01:55:37", "09:29:42", "17:21:00", "22:56:19", "04:45:39", "04:36:18" };
            var palindromicMinutes = new int[] { 00, 11, 22, 33, 44, 55 };
            var sumOfDifferences = 0;

            foreach (var time in times)
            {
                var currentDateTime = DateTime.ParseExact(time, "HH:mm:ss", CultureInfo.CurrentCulture);
                var units = time.Split(':');
                var hours = int.Parse(units[0]);
                var minutes = int.Parse(units[1]);

                // Find the closest palidromic mintues
                var isNewHour = false;
                var closestpalindromicMinute = 0;
                var closest = 60;
                foreach (var palindromicMinute in palindromicMinutes)
                {
                    var lowerDifference = Math.Abs(minutes - palindromicMinute);
                    var upperDifference = Math.Abs(minutes - (palindromicMinute + 60));
                    var smallestDifference = Math.Min(lowerDifference, upperDifference);
                    
                    if (smallestDifference < closest)
                    {
                        closest = smallestDifference;
                        closestpalindromicMinute = palindromicMinute;
                        isNewHour = palindromicMinute == 0 && upperDifference < lowerDifference;
                    }
                }

                // Account for times with hours that cannot be made into palindromes (due to the need to exceed 59 seconds)
                // TODO: This can be done ahead of all the previous logic
                var unableToPalidromeAMFloor = DateTime.ParseExact("05:55:50", "HH:mm:ss", CultureInfo.CurrentCulture);
                var unableToPalindromeAMCeiling = DateTime.ParseExact("10:00:01", "HH:mm:ss", CultureInfo.CurrentCulture);
                var unableToPalindromAMMiddle = unableToPalidromeAMFloor + (unableToPalindromeAMCeiling - unableToPalidromeAMFloor) / 2;
                var unableToPalindromePMFloor = DateTime.ParseExact("15:55:51", "HH:mm:ss", CultureInfo.CurrentCulture);
                var unableToPalindromePMCeiling = DateTime.ParseExact("20:00:02", "HH:mm:ss", CultureInfo.CurrentCulture);
                var unableToPalindromePMMiddle = unableToPalindromePMFloor + (unableToPalindromePMCeiling - unableToPalindromePMFloor) / 2;

                DateTime palindromicDateTime = default;
                if (isNewHour && hours == 23)
                {
                    palindromicDateTime = DateTime.Today.AddDays(1);
                }
                else if (currentDateTime.Between(unableToPalidromeAMFloor, unableToPalindromAMMiddle))
                {
                    palindromicDateTime = unableToPalidromeAMFloor;
                }
                else if (currentDateTime.Between(unableToPalindromAMMiddle, unableToPalindromeAMCeiling))
                {
                    palindromicDateTime = unableToPalindromeAMCeiling;
                }
                else if (currentDateTime.Between(unableToPalindromePMFloor, unableToPalindromePMMiddle))
                {
                    palindromicDateTime = unableToPalindromePMFloor;
                }
                else if (currentDateTime.Between(unableToPalindromePMMiddle, unableToPalindromePMCeiling))
                {
                    palindromicDateTime = unableToPalindromePMCeiling;
                }
                // If the hour can be reversed into seconds, create the new time with the previously found minutes
                else
                {
                    // Increment the hour if the minutes need to move up to '00'
                    if (isNewHour) hours++;

                    var newSecondsArray = hours.ToString().PadLeft(2, '0').ToCharArray();
                    var newSeconds = new string(new char[] { newSecondsArray.Last(), newSecondsArray.First()});
                    
                    var newTime = $"{hours.ToString().PadLeft(2, '0')}:{closestpalindromicMinute.ToString().PadLeft(2, '0')}:{newSeconds.ToString().PadLeft(2, '0')}";
                    palindromicDateTime = DateTime.ParseExact(newTime, "HH:mm:ss", CultureInfo.CurrentCulture);
                }

                var difference = (currentDateTime - palindromicDateTime).Duration().TotalSeconds;
                sumOfDifferences += (int)difference;

                // Console.WriteLine($"{currentDateTime.ToString("HH:mm:ss")} - {palindromicDateTime.ToString("HH:mm:ss")} - {difference}");
            }

            Console.WriteLine(sumOfDifferences);
        }
    }

    public static class PalindromeTimesExtensionMethods
    { 
        // Make generic
        public static bool Between(this DateTime current, DateTime floor, DateTime ceiling) 
        {
            return current >= floor && current <= ceiling;
        }
    }

}

