using System;
using System.Collections.Generic;
using System.Linq;
using TuneMusix.Data;
using TuneMusix.Data.SQLDatabase;
using TuneMusix.Helpers;
using TuneMusix.Model;
using WinForms = System.Windows.Forms;

namespace TuneMusix.ViewModel
{
    partial class ViewModelMain
    {   
        

        /// <summary>
        /// Opens a File Dialog to select Audio Files
        /// </summary>
        /// <param name="parameter"></param>
        private void getFiles(object parameter)
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
        private void deleteTracks(object parameter)
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
        public void exitApplication(object argument)
        {
            loader = new SQLLoader();
            loader.SaveOptions();
            audioControls.Dispose();
            System.Windows.Application.Current.Shutdown();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        public void debugMethod(object argument)
        {
            Console.WriteLine("--");
            if (Playlists != null)
            {
                Console.WriteLine("Playlists:" + Playlists.Count);
            }
            Console.WriteLine("--");
            Console.WriteLine("Playlists:");
            if (Playlists != null)
            {
                foreach (Playlist p in Playlists)
                {
                    Console.WriteLine("Name: " + p.Name + " - - ID: " + p.ID);
                }          
            }
            Console.WriteLine("--");
            if (SelectedPlaylist != null)
            {
                Console.WriteLine("Selected Playlist: " + SelectedPlaylist.Name);
            }
            Console.WriteLine("--");
            if (SelectedTracks!=null)
            {
                Console.WriteLine("Selected Track:");
                foreach (Track track in SelectedTracks)
                {
                    Console.WriteLine(" - " + track);
                }
            }
            Console.WriteLine("--");
            if (SelectedFolder!=null)
            {
                Console.WriteLine("Selected Folder: " + SelectedFolder.Name);
            }
            Console.WriteLine("--");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        public void addFolder(object argument)
        {
            var folderbrowser = new WinForms.FolderBrowserDialog();

            if (folderbrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(folderbrowser.SelectedPath))
            {
                dataModel.AddFolderFromFileURL(folderbrowser.SelectedPath);
            }           
        }


    }
}
