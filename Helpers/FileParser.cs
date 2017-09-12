using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Model;

namespace TuneMusix.Helpers
{
    public class FileParser
    {
        /// <summary>
        /// Reads the Data from a given URL for Audiofiles
        /// </summary>
        /// <param name="url">URL of the Audiofile</param>
        /// <returns>Track</returns>
        public Track GetAudioData(string url)
        {

            Track track = new Track(url);

            byte[] b = new byte[128];

            FileStream fs = new FileStream(url, FileMode.Open);
            fs.Seek(-128, SeekOrigin.End);
            fs.Read(b, 0, 128);
            bool isSet = false;
            String sFlag = System.Text.Encoding.Default.GetString(b, 0, 3);
            if (sFlag.CompareTo("TAG") == 0)
            {
                isSet = true;
            }

            if (isSet)
            {
                //get   title   of   song; 
                track.tTitle = RemoveControlCharacters(System.Text.Encoding.Default.GetString(b, 3, 30));

                //get   interpret; 
                track.tInterpret = RemoveControlCharacters(System.Text.Encoding.Default.GetString(b, 33, 30));

                //get   album; 
                track.tAlbum = RemoveControlCharacters(System.Text.Encoding.Default.GetString(b, 63, 30));

                //get   Year; 
                track.tYear = RemoveControlCharacters(System.Text.Encoding.Default.GetString(b, 93, 4));

                //get   Comment; 
                track.tComm = RemoveControlCharacters(System.Text.Encoding.Default.GetString(b, 97, 30));

                //get Genre
                track.tGenre = RemoveControlCharacters(System.Text.Encoding.Default.GetString(b, 127, 1));

            }

            return track;
        }



        public string RemoveControlCharacters(string inString)
        {
            if (inString == null) return "";
            StringBuilder newString = new StringBuilder();
            foreach (Char c in inString)
            {
                if (!char.IsControl(c))
                {
                    newString.Append(c);
                }
            }
            return newString.ToString();
        }
    }
}
