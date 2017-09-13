﻿using System;
using System.Collections.Generic;
using System.Linq;
using TuneMusix.Helpers;
using TuneMusix.Model;
using WinForms = System.Windows.Forms;

namespace TuneMusix.ViewModel
{
    partial class ViewModelMain
    {
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
                Logger.Log(Convert.ToString(ofd.FileNames.Length) + " files were selected.");

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
            Console.WriteLine("------------------------------" + dataModel.RootFolders.Count);
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
