using System;
using System.Collections.Generic;
using System.Linq;
using TuneMusix.Data;
using TuneMusix.Helpers;
using TuneMusix.Model;
using WinForms = System.Windows.Forms;

namespace TuneMusix.ViewModel
{
    partial class ViewModelMain
    {
        /// <summary>
        /// Saves Options,Tracks and Folders to the database
        /// </summary>
        /// <param name="parameter"></param>
        private void _save(object parameter)
        {
            SQLManager DBmanager = new SQLManager();
            DBmanager.UpdateOptions(IDGenerator.IDCounter,options);
            //add folders to database
            foreach (Folder folder in RootFolders)
            {
                DBmanager.AddFolder(folder);
            }
            //add tracks to database
            DBmanager.AddTrackList(TrackList.ToList<Track>());
            //add playlists to database
            foreach (Playlist p in Playlists)
            {
                DBmanager.AddPlaylist(p);
            }
            DBmanager.UpdateOptions(IDGenerator.IDCounter,options);
        }
        /// <summary>
        /// 
        /// </summary>
        private void LoadFromDB()
        {
            SQLManager DBmanager = new SQLManager();
            //Load options
            IDGenerator IDgen = IDGenerator.Instance;
            long idgen = DBmanager.GetIDCounterStand();
            if (idgen == 0 || idgen == 1)
            {
                idgen = 2;
            }
            IDgen.Initialize(idgen);
            options = DBmanager.GetOptions();
            //Load all folders
            List<Folder> FolderList = DBmanager.GetFolders();
            List<Folder> RootList = new List<Folder>();
            Console.WriteLine("Folders loaded");

            //load all tracks
            Console.WriteLine("Loading Tracks");
            List<Track> tracklist = DBmanager.GetTracks();
            dataModel.AddTracksDB(tracklist);
            Console.WriteLine("Tracks loaded");         
            FolderSort(FolderList);
            TrackSort(FolderList);
            foreach (Folder folder in FolderList)
            {
                if (folder.FolderID == 1)
                {
                    RootList.Add(folder);
                }
            }
            dataModel.AddRootFoldersDB(RootList);
            Console.WriteLine("Loading finished");
        }

        private List<Folder> FolderSort(List<Folder> FolderList)
        {
            List<Folder> templist = FolderList;
            foreach (Folder a in templist)
            {
                foreach (Folder b in templist)
                {
                    if (a.ID == b.FolderID)
                    {
                        a.InsertFolder(b);
                    }
                }
            }
            return templist;
        }
        private List<Folder> TrackSort(List<Folder> FolderList)
        {
            List<Folder> tempFolderList = FolderList;
            foreach (Track track in TrackList)
            {
                foreach (Folder folder in FolderList)
                {
                    if (track.FolderID == folder.ID)
                    {
                        folder.AddTrack(track);
                    }
                }
            }
            return tempFolderList;
        }

        /// <summary>
        /// Adds the selected tracks to the selected playlist
        /// </summary>
        /// <param name="parameter"></param>
        private void _addToPlaylist(object parameter)
        {
            if (SelectedPlaylist != null)
            {
                SelectedPlaylist.AddAllTracks(SelectedTracks.ToList<Track>());
                RaisePropertyChanged("SelectedPlayList");//necessary?
            }
        }

        /// <summary>
        /// Opens a File Dialog to select Audio Files
        /// </summary>
        /// <param name="parameter"></param>
        private void _getFiles(object parameter)
        {

            var ofd = new WinForms.OpenFileDialog();
            ofd.Title = "File Browser";
            ofd.InitialDirectory = @"C:\"; //change to systems default hd
            ofd.Filter = "mp3 file (*.mp3)|*.mp3"; //| wav file (*.wav)|*wav
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<string> URLList = new List<string>();
                //for every File selected
                foreach (String url in ofd.FileNames)
                {
                    URLList.Add(url);
                }
                dataModel.AddTracksFromFileURLs(URLList);
            }
        }

        /// <summary>
        /// Deletes all selected tracks from the tracklist
        /// </summary>
        /// <param name="parameter"></param>
        private void _deleteTracks(object parameter)
        {

            List<Track> deleteList = new List<Track>();

            foreach (Track selectedTrack in SelectedTracks)
            {
                selectedTrack.Dispose();
            }
            RaisePropertyChanged("TrackList");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        public void _exitApplication(object argument)
        {
            System.Windows.Application.Current.Shutdown();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        public void _debugMethod(object argument)
        {
        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        public void _addFolder(object argument)
        {
            var folderbrowser = new WinForms.FolderBrowserDialog();

            if (folderbrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(folderbrowser.SelectedPath))
            {
                dataModel.AddFolderFromFileURL(folderbrowser.SelectedPath);
            }           
        }


    }
}
