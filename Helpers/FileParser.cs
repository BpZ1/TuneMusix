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
            try
            {
                if (url == null) return null;
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
                    track.Title = RemoveControlCharacters(System.Text.Encoding.Default.GetString(b, 3, 30)).Trim();

                    //get   interpret; 
                    track.Interpret = RemoveControlCharacters(System.Text.Encoding.Default.GetString(b, 33, 30)).Trim();

                    //get   album; 
                    track.Album = RemoveControlCharacters(System.Text.Encoding.Default.GetString(b, 63, 30)).Trim();

                    //get   Year; 
                    track.Year = RemoveControlCharacters(System.Text.Encoding.Default.GetString(b, 93, 4)).Trim();

                    //get   Comment; 
                    track.Comm = RemoveControlCharacters(System.Text.Encoding.Default.GetString(b, 97, 30)).Trim();

                    //get Genre
                    track.Genre = RemoveControlCharacters(System.Text.Encoding.Default.GetString(b, 127, 1)).Trim();

                }
                return track;
            }
            catch (Exception e)
            {
                //show to user with url
                Logger.LogException(e);
                return null;
            }
        }
        /// <summary>
        /// Adds all Files and Folders contained in the given URL to the DataModel.
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public Folder GetFolderData(string URL)
        {
            //Add new root Folder
            Folder root = new Folder(Path.GetFileNameWithoutExtension(URL),URL);

            List<Track> tracks = new List<Track>();
            string[] files = Directory.GetFiles(URL);
            string[] dirs = Directory.GetDirectories(URL);
            //Add all files in the firectory
            foreach (string url in files)
            {
                string extention = Path.GetExtension(url);
                if (extention.Equals(".mp3"))
                {
                    Track mp3 = GetAudioData(url);
                    tracks.Add(mp3);
                    root.AddTrack(mp3);
                }
            }
            //Recursion for subdirectories
            foreach (string dir in dirs)
            { 
                //Add subfolder to root folder
                Folder fold = GetFolderData(dir);
                root.AddFolder(fold);
            }
            return root;
        }

        /// <summary>
        /// Removes all control characters from a given string.
        /// </summary>
        /// <param name="inString"></param>
        /// <returns></returns>
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
