using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


    }
}
