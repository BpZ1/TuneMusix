using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuneMusix.Attributes;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.Data.DataModelOb
{
    /// <summary>
    /// Contains all Methods that call the parser to
    /// create the initial data.
    /// </summary>
    public partial class DataModel
    {
        
        /// <summary>
        /// Adds a list of tracks from their URLs to the datamodel and the database.
        /// </summary>
        /// <param name="urls"></param>
        public void AddTracksFromFileURLs(List<string> urls)
        {
            int addedCounter = 0;
            int notAddedCounter = 0;
            List<Track> Tracklist = new List<Track>();
            foreach (string url in urls)
            {
                bool contained = false;
                foreach (Track track in TrackList)
                {
                    if (track.sourceURL.Equals(url))
                    {
                        contained = true;
                        notAddedCounter++;
                    }
                }
                if (!contained)
                {
                    FileParser fileParser = new FileParser();
                    Track mp3 = fileParser.GetAudioData(url);
                    if (mp3 != null)
                    {
                        addedCounter++;
                        mp3.IsModified = false;
                        TrackList.Add(mp3);
                        Tracklist.Add(mp3);
                        OnDataModelChanged();
                    }
                }
            }
            DBManager.AddAll(Tracklist);
            DialogService.NotificationMessage(addedCounter + " tracks have been added.");
            if (notAddedCounter > 0)
            {
                DialogService.NotificationMessage(notAddedCounter + "could not be added because they already exist.");
            }
        }

        /// <summary>
        /// Adds a root folder and all of its content to the model/database
        /// </summary>
        /// <param name="url"></param>
        public void AddFolderFromFileURL(string url)
        {
            int trackCount = TrackList.Count;
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
                //Show message that foder is already in an existing folder
                DialogService.NotificationMessage("The folder you tried to add is already contained in an existing folder.");
            }
            else if (!IsSubFolder)
            {
                FileParser fileParser = new FileParser();
                Folder folder = fileParser.GetFolderData(url);
                folder.FolderID = 1;                
                AddRootFolder(folder);
                folder.IsModified = false;
                //Show Popup that the folder was added (a small popup on the side?)
                if (IsRootFolder && SubFolder != null)
                {
                    Delete(SubFolder);
                    DialogService.NotificationMessage("The folder you added is the rootfolder of an existing folder.");
                    //Show Popup that the folder was added and is root to another (a small popup on the side?)
                    // RootFolders.Remove(RootFolder);
                    //folder.AddFolder(RootFolder);        Parse only the data that is not already in the database later?        
                    OnDataModelChanged();                 
                }
                DialogService.NotificationMessage(TrackList.Count - trackCount + " tracks have been added.");
            }
        }
      

    }
}
