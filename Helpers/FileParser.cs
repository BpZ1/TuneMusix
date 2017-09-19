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
        IDGenerator IDgen;

        public FileParser()
        {
            IDgen = IDGenerator.Instance;
        }

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
                Track track = new Track(url,IDgen.GetID());
                byte[] b = new byte[128];



                TagLib.File f = TagLib.File.Create(url);
                track.Title = f.Tag.Title;
                track.Interpret = f.Tag.FirstAlbumArtist;
                track.Album = f.Tag.Album;
                track.Comm = f.Tag.Comment;
                track.Genre = f.Tag.FirstGenre;
                track.Year = (int)f.Tag.Year;
              
                return track;
            }
            catch (UnauthorizedAccessException ex)
            {
                Logger.LogException(ex);
                return null;
            }
            catch (FileNotFoundException ex)
            {
                Logger.LogException(ex);
                return null;
            }
            catch (IOException ex)
            {
                Logger.LogException(ex);
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
            string[] URLs = URL.Split('\\');
            Folder root = new Folder(URLs.Last(), URL, IDgen.GetID());

            string[] files = Directory.GetFiles(URL);
            string[] dirs = Directory.GetDirectories(URL);
            //Add all files in the firectory
            foreach (string url in files)
            {
                string extention = Path.GetExtension(url);
                if (extention.Equals(".mp3"))
                {
                    Track mp3 = GetAudioData(url);
                    if(mp3 != null)
                    {
                        root.AddTrack(mp3);
                    }               
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

    }
}
