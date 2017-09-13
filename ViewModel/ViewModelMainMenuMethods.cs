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
            foreach (Track t in TrackList)
            {
                if (t.FolderID == -1)
                {
                    DBmanager.AddTrack(t);
                }
            }
            foreach (Playlist p in Playlists)
            {
                DBmanager.AddPlaylist(p);
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

        public void _exitApplication(object argument)
        {
            System.Windows.Application.Current.Shutdown();
        }

        public void _debugMethod(object argument)
        {
            Console.WriteLine("DBTEST");
            Console.WriteLine("DBTEST");
            SQLManager manager = new SQLManager();
            foreach (Track t in TrackList)
            {
                manager.AddTrack(t);
            }
            List<Track> tracks = manager.GetTracks();
            foreach (Track t in tracks)
            {
                Console.WriteLine(t.Name + " - " + t.url);
            }
        }


        public void _addFolder(object argument)
        {
            var folderbrowser = new WinForms.FolderBrowserDialog();

            if (folderbrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(folderbrowser.SelectedPath))
            {
                dataModel.AddFolder(folderbrowser.SelectedPath);
            }           
        }


    }
}
