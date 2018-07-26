using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TuneMusix.Model;

namespace TuneMusix.Helpers
{
    public static class TrackService
    {
        /// <summary>
        /// Checks if any of the Track properties contains the given string.
        /// Checked are name, interpret and album.
        /// </summary>
        /// <param name="t">Track to be searched</param>
        /// <param name="s">string to be looked up</param>
        /// <param name="caseSensitive">Case sensitive</param>
        /// <returns></returns>
        public static bool Contains(Track t, string s,bool caseSensitive)
        {
            if (caseSensitive)
            {
                if (t.Album.Contains(s))
                    return true;

                if (t.Interpret.Contains(s))
                    return true;

                if (t.Title.Contains(s))
                    return true;

                return false;
            }
            else
            {
                if (t.Album.ToLower().Contains(s.ToLower()))
                    return true;

                if (t.Interpret.ToLower().Contains(s.ToLower()))
                    return true;

                if (t.Title.ToLower().Contains(s.ToLower()))
                    return true;

                return false;
            }    
        }

        /// <summary>
        /// Adds two durations in the form XX:XX:XX together.
        /// </summary>
        /// <param name="duration1">Duration 1</param>
        /// <param name="duration2">Duration 2</param>
        /// <returns>Duration as string in the form XX:XX:XX or if longer than 24 hours:
        /// XX:XX:XX:XX.</returns>
        public static string AddDurations(string duration1, string duration2)
        {
            if(duration1 == null || duration2 == null)
            {
                throw new ArgumentNullException("The string for the addition of durations can't be null");
            }

            string[] times1 = duration1.Split(':');
            string[] times2 = duration2.Split(':');

            int[] times = new int[4];
            int counter = 3;
            for(int i = 5; i > 1; i--)
            {
                Console.Out.WriteLine("");
            }
            for(int i = times1.Length - 1; i > -1; i--)
            {
                int number = 0;
                if (!times1[i].Equals(""))
                {
                    number = Int32.Parse(times1[i]);
                }
                times[counter] += number;
                counter--;
            }
            counter = 3;
            for (int i = times2.Length - 1; i > -1; i--)
            {
                int number = 0;
                if (!times2[i].Equals(""))
                {
                    number = Int32.Parse(times2[i]);
                }
                times[counter] += number;
                counter--;
            }


            int seconds = times[3];
            int minutes = times[2];
            int hours = times[1];
            int days = times[0];


            if(seconds >= 60)
            {
                minutes += seconds / 60;
                seconds = seconds % 60;
            }
            if (minutes >= 60)
            {
                hours += minutes / 60;
                minutes = minutes % 60;
            }
            if (hours >= 24)
            {
                days += hours / 24;
                hours = hours % 24;
            }
            string result = "";

            if (days > 0)
            {
                result += days + ":";
            }
            if(hours > 0)
            {
                if(days > 0 && hours < 10)
                {
                    result += "0" + hours + ":";
                }
                else
                {
                    result += hours + ":";
                }
            }
            else
            {
                result += "00:";
            }
            if(minutes > 0)
            {
                if(minutes < 10)
                {
                    result += "0" + minutes+ ":";
                }
                else
                {
                    result += minutes + ":";
                }

            }
            else
            {
                result += "00:";
            }
            if (seconds > 0)
            {
                if (seconds < 10)
                {
                    result += "0" + seconds;
                }
                else
                {
                    result += seconds;
                }

            }
            else
            {
                result += "00";
            }
            return result;
        }

    }
}
