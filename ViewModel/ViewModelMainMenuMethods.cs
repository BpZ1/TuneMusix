using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using TuneMusix.Data.SQLDatabase;
using TuneMusix.Helpers.Dialogs;
using TuneMusix.Model;
using TuneMusix.View.OptionsWindow;
using TuneMusix.ViewModel.Dialog;
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
                dataModel.AddTracks(URLList);
            }

            watch.Stop();
            Debug.WriteLine("Time: " + watch.ElapsedMilliseconds);
        }

        private void openOptionsWindow(object argument)
        {
            var win = new OptionsWindowView();

            win.ShowDialog();
        }
        /// <summary>
        /// Saved the changed data to the database.
        /// </summary>
        /// <param name="argument"></param>
        private void saveData(object argument)
        {
            //get modified files from the datamodel
            List<Folder> saveFolders = dataModel.GetAllFolders(true);
            List<Playlist> savePlaylists = checkPlaylistsModified();
            List<Track> saveTracks = checkTracksModified();

            int modifiedCount = saveTracks.Count + savePlaylists.Count + saveFolders.Count;

          
            if (modifiedCount > 0)
            {              
                //Save data to database
                Database dbManager = Database.Instance;

                dataModel.SavePlaylists(savePlaylists);
                dataModel.SaveTracks(saveTracks);
                dataModel.SaveFolders(saveFolders);

                dbManager.Insert(saveTracks);
                dbManager.Insert(saveFolders);
                DialogService.NotificationMessage(modifiedCount + " files have been saved.");
            }
                

            
            Debug.WriteLine("Modified files have been saved.");
        }

        /// <summary>
        /// Returns a list of all modified tracks in the datamodel.
        /// </summary>
        /// <returns></returns>
        private List<Track> checkTracksModified()
        {
            List<Track> modifiedTracks = new List<Track>();
            foreach (Track track in dataModel.TrackList)
            {
                if (track.IsModified)
                    modifiedTracks.Add(track);

            }
            return modifiedTracks;

        }
        /// <summary>
        /// Returns a list of all modified playlists in the datamodel.
        /// </summary>
        /// <returns></returns>
        private List<Playlist> checkPlaylistsModified()
        {
            List<Playlist> modifiedPlaylists = new List<Playlist>();
            foreach (Playlist playlist in dataModel.Playlists)
            {
                if (playlist.IsModified)
                    modifiedPlaylists.Add(playlist);
            }

            return modifiedPlaylists;
        }

        private void exitButtonPressed(object argument)
        {
            var window = argument as Window;
            window.Close();
        }

        /// <summary>
        /// Gets called when the exit button is pressed or the window is closed.
        /// </summary>
        /// <param name="argument"></param>
        private void exitApplication(object argument)
        {
            var eventArgs = argument as CancelEventArgs;

            int count = dataModel.GetAllFolders(true).Count + checkPlaylistsModified().Count + checkTracksModified().Count;
            //Ask if data should be saved.
            if (count > 0 && options.AskConfirmation)
            {
                DialogViewModelBase vm = new ConfirmationDialogViewModel("Do you want to close without saving?");
                DialogResult result = DialogService.OpenDialog(vm);

                if(result == DialogResult.Yes)
                {
                    eventArgs.Cancel = false;
                    //options have to be saved as they are not always saved as they are changed.
                    Options.Instance.SaveValues();
                    //audiControl has to be disposed to end playing of music.
                    audioControls.Dispose();
                }                
                else if(result == DialogResult.No)
                {
                    eventArgs.Cancel = true;
                    return;
                }
                else
                {
                    eventArgs.Cancel = true;
                    return;
                }
            }
            else
            {
                Options.Instance.SaveValues();
                //audiControl has to be disposed to end playing of music.
                audioControls.Dispose();
            }
        }

        /// <summary>
        /// Adds the folder via the selected URL to the datamodel and database.
        /// </summary>
        /// <param name="argument"></param>
        public void addFolder(object argument)
        {
            var watch = new Stopwatch();
            watch.Start();

            var folderbrowser = new WinForms.FolderBrowserDialog();

            if (folderbrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(folderbrowser.SelectedPath))
            {
                
                dataModel.AddTracks(folderbrowser.SelectedPath);
            }

            watch.Stop();
            Debug.WriteLine("Time: " + watch.ElapsedMilliseconds);
        }


    }
}
