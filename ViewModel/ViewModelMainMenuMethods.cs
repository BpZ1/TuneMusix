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
        /// 
        /// </summary>
        /// <param name="argument"></param>
        public void _exitApplication(object argument)
        {
            dataModel.SaveOptions(IDGenerator.IDCounter);
            audioControls.Dispose();
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
