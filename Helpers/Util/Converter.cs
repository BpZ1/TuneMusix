﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneMusix.Helpers
{
    /// <summary>
    /// This class contains conversion methods for differtent objects.
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// Converts an integer into a bool.
        /// </summary>
        /// <param name="boolean"></param>
        /// <returns></returns>
        public static int BoolToIntConverter(bool boolean)
        {
            if (boolean)
                return 1;

            return 0;
        }
        /// <summary>
        /// Converts a bool into an integer.
        /// </summary>
        /// <param name="integer"></param>
        /// <returns></returns>
        public static bool IntToBoolConverter(int integer)
        {
            if (integer > 1 || integer < 0)
                throw new ArgumentOutOfRangeException();

            if (integer == 1)
                return true;

            return false;
        }

        public static string TimeSpanToString(TimeSpan timeSpan)
        {
            if (timeSpan == null)
                throw new ArgumentNullException();

            int hours = timeSpan.Hours;
            int minutes = timeSpan.Minutes;
            int seconds = timeSpan.Seconds;
            string result = "";
            if(hours != 0)
            {
                result += hours + ":";
            }
            if(minutes != 0)
            {
                if(hours != 0 && minutes < 10)
                {
                    result += (minutes * 10) + ":";
                }
                else
                {
                    result += minutes + ":";
                }
            }
            if(seconds < 10)
            {
                if(seconds == 0)
                {
                    result += "00";
                }
                else
                {
                    result += seconds + "0";
                }
            }
            else
            {
                result += seconds;
            }
            return result;
        }
    }
}
