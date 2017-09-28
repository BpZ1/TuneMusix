using System;
using System.Collections.Generic;
using System.Diagnostics;
using TuneMusix.Data.SQLDatabase;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer.Effects;
using TuneMusix.Model;
using TuneMusix.View.OptionsWindow;
using TuneMusix.ViewModel.Effects;
using WinForms = System.Windows.Forms;

namespace TuneMusix.ViewModel
{
    partial class ViewModelMain
    {   
        

        /// <summary>
        /// Opens a File Dialog to select Audio Files
        /// </summary>
        /// <param name="parameter"></param>
        private void _getFiles(object parameter)
        {
            var watch = new Stopwatch();
            watch.Start();

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

            watch.Stop();
            Debug.WriteLine("Time: " + watch.ElapsedMilliseconds);
        }

        private void _openOptionsWindow(object argument)
        {
            var win = new OptionsWindowView();

            win.Show();
        }

        private void _saveData(object argument)
        {
            int count = 0;
            List<Track> saveTracks = new List<Track>();
            List<Folder> saveFolders = dataModel.GetAllFolders(true);
            List<Playlist> savePlaylists = new List<Playlist>();
            SQLManager dbManager = new SQLManager();
            //get modified tracks from the datamodel
            foreach (Track track in dataModel.TrackList)
            {
                if (track.IsModified)
                {
                    saveTracks.Add(track);
                    track.IsModified = false;
                    count++;
                }
                
            }
            foreach(Playlist playlist in dataModel.Playlists)
            {
                if (playlist.IsModified)
                {
                    dbManager.AddPlaylist(playlist);
                    playlist.IsModified = false;
                    count++;
                }
            }      
            dbManager.AddAll(saveTracks);
            dbManager.AddAll(saveFolders);
            count += saveFolders.Count;
            foreach(Track t in saveTracks)
            {
                Console.WriteLine(t.Rating);
            }
            if (count > 0)
            {
            DialogService.NotificationMessage(count + " files have been saved.");
            }
            Debug.WriteLine("Modified files have been saved.");
        }

        /// <summary>
        /// Gets called when the exit button is pressed or the window is closed.
        /// </summary>
        /// <param name="argument"></param>
        public void _exitApplication(object argument)
        {
            //Ask if data should be saved.


            //options have to be saved as they are not always saved as they are changed.
            dataModel.SaveOptions(IDGenerator.IDCounter);
            //audiControl has to be disposed to end playing of music.
            audioControls.Dispose();
            System.Windows.Application.Current.Shutdown();
        }
        /// <summary>
        /// Method used for debugging.
        /// </summary>
        /// <param name="argument"></param>
        public void _debugMethod(object argument)
        {
            dataModel.AddEffectToQueue(new FlangerEffect());
        }

        /// <summary>
        /// Adds the folder via the selected URL to the datamodel and database.
        /// </summary>
        /// <param name="argument"></param>
        public void _addFolder(object argument)
        {
            var watch = new Stopwatch();
            watch.Start();

            var folderbrowser = new WinForms.FolderBrowserDialog();

            if (folderbrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(folderbrowser.SelectedPath))
            {
                dataModel.AddFolderFromFileURL(folderbrowser.SelectedPath);
            }

            watch.Stop();
            Debug.WriteLine("Time: " + watch.ElapsedMilliseconds);
        }


    }
}
