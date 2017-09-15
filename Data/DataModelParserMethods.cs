using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Attributes;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.Data
{
    /// <summary>
    /// 
    /// Contains all Methods that call the parser to
    /// create the initial data.
    /// 
    /// </summary>
    public partial class DataModel
    {
        /// <summary>
        /// Checks if a Track is already in the List and then adds it.
        /// </summary>
        /// <param name="URL"></param>
        public bool AddTrackFromFileURL(string URL)
        {
            if (URL == null) { return false; }
            bool contained = false;
            foreach (Track track in TrackList)
            {
                if (track.url.Equals(URL))
                {
                    contained = true;
                }
            }
            if (contained == false)
            {
                FileParser fileParser = new FileParser();
                Track mp3 = fileParser.GetAudioData(URL);
                if (mp3 != null)
                {
                    TrackList.Add(mp3);
                    OnDataModelChanged();
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Adds a root folder and all of its content to the model/database
        /// </summary>
        /// <param name="url"></param>
        public void AddFolderFromFileURL(string url)
        {
            bool contained = false;
            foreach (Folder f in RootFolders)
            {
                if (url.Contains(f.URL))
                {
                    contained = true;
                }
            }
            if (!contained)
            {
                FileParser fileParser = new FileParser();
                Folder folder = fileParser.GetFolderData(url);
                folder.FolderID = 1;
                AddRootFolder(folder);
                AddFolderContent(folder);
            }
            else
            {
                Console.WriteLine("Folder already contained");
                //////Show message that foder is already in an existing folder
            }
        }
    }
}
