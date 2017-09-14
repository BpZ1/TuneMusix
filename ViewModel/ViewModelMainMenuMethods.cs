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
            foreach (Folder folder in RootFolders)
            {
                DBmanager.AddFolder(folder);
            }
            Console.WriteLine("Folders Saved");
            foreach (Track t in TrackList)
            {
                if (t.FolderID == -1)
                {
                    DBmanager.AddTrack(t);
                }
            }
            Console.WriteLine("Tracks Saved");
            foreach (Playlist p in Playlists)
            {
                DBmanager.AddPlaylist(p);
            }
            DBmanager.UpdateOptions(IDGenerator.IDCounter,options);
        }

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
            List<Folder> tempList = DBmanager.GetFolders();
            List<Folder> templist2 = tempList;
            Console.WriteLine("Folders loaded");
            foreach (Folder f in tempList)
            {
                Console.WriteLine("Folder ID: " + f.ID + "   Folder folderID: " + f.FolderID);
            }
            //load all tracks
            foreach (Track track in DBmanager.GetTracks())
            {
                if (track != null)
                {
                    TrackList.Add(track);
                    foreach (Folder folder in tempList)
                    {
                        if (track.FolderID == folder.ID || track.FolderID != -1)//change ID to 0----------------------------------------------------------------
                        {
                            folder.AddTrack(track);
                        }
                    }
                }    
            }
            Console.WriteLine("Tracks loaded");
            foreach (Folder folder in tempList)
            {
                foreach(Folder f in templist2)
                {
                    if (folder.ID == f.FolderID || folder.FolderID != -1 || f != folder)//change ID to 0----------------------------------------------------------------
                    {
                        folder.AddFolder(f);
                        //f.AddFolder(folder);
                    }
                }
            }
            Console.WriteLine("Tracks loaded");
            foreach (Folder folder in tempList)
            {
                if (folder.FolderID == -1)//change ID to 0----------------------------------------------------------------
                {
                    dataModel.AddRootFolder(folder);
                }
            }
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
                //for every File selected
                foreach (String url in ofd.FileNames)
                {
                    dataModel.AddTrackURL(url);
                }
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
                //binary search when sorted or better solution
                Console.WriteLine(selectedTrack.Title);
                foreach (Track track in TrackList)
                {
                    if (track.url.Equals(selectedTrack.url))
                    {
                        deleteList.Add(track);
                    }
                }

            }
            foreach (Track t in deleteList)
            {
                TrackList.Remove(t);
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
            for (int i = 0;i<=20;i++)
            {
                IDGenerator gen = IDGenerator.Instance;
                Console.WriteLine(gen.GetID());
            }           
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
                dataModel.AddRootFolder(folderbrowser.SelectedPath);
            }           
        }


    }
}
