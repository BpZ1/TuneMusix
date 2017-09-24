using System;
using System.Collections.Generic;
using TuneMusix.Helpers;
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
        /// Gets called when the exit button is pressed or the window is closed.
        /// </summary>
        /// <param name="argument"></param>
        public void _exitApplication(object argument)
        {
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
  

        }

        /// <summary>
        /// Adds the folder via the selected URL to the datamodel and database.
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
