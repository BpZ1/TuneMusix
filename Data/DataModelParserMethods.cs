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
        /// 
        /// </summary>
        /// <param name="urls"></param>
        public void AddTracksFromFileURLs(List<string> urls)
        {
            List<Track> Tracklist = new List<Track>();
            foreach (string url in urls)
            {
                bool contained = false;
                foreach (Track track in TrackList)
                {
                    if (track.url.Equals(url))
                    {
                        contained = true;
                    }
                }
                if (!contained)
                {
                    FileParser fileParser = new FileParser();
                    Track mp3 = fileParser.GetAudioData(url);
                    if (mp3 != null)
                    {
                        TrackList.Add(mp3);
                        Tracklist.Add(mp3);
                        OnDataModelChanged();
                    }
                }
            }
            DBManager.AddAll(Tracklist);
        }

        /// <summary>
        /// Adds a root folder and all of its content to the model/database
        /// </summary>
        /// <param name="url"></param>
        public void AddFolderFromFileURL(string url)
        {
            bool IsSubFolder = false;
            bool IsRootFolder = false;
            Folder SubFolder = null;
            foreach (Folder f in RootFolders)
            {
                if (url.Contains(f.URL))
                {
                    IsSubFolder = true;
                }
                if (f.URL.Contains(url) && !f.URL.Equals(url))
                {
                    IsRootFolder = true;
                    SubFolder = f;
                }
            }
            if (IsSubFolder)
            {
                Console.WriteLine("Folder already contained");
                //////Show message that foder is already in an existing folder
            }
            else if (!IsSubFolder)
            {
                FileParser fileParser = new FileParser();
                Folder folder = fileParser.GetFolderData(url);
                folder.FolderID = 1;
                AddRootFolder(folder);
                //Show Popup that the folder was added (a small popup on the side?)
                if (IsRootFolder && SubFolder != null)
                {
                    Console.WriteLine("Is Root of " + SubFolder.Name);
                    Delete(SubFolder);
                    Console.WriteLine("Deleted");
                    //Show Popup that the folder was added and is root to another (a small popup on the side?)
                    // RootFolders.Remove(RootFolder);
                    //folder.AddFolder(RootFolder);        Parse only the data that is not already in the database later?        
                    OnDataModelChanged();
                }
            }
        }
    }
}
