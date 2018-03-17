using System;
using System.ComponentModel;

namespace TuneMusix.ViewModel
{
    partial class ViewModelMain
    {

        private void onRootFoldersChanged(object source,object obj)
        {
             RaisePropertyChanged("TrackList");
        }

        private void onLoadingComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            audioControls.LoadEffects();
        }

        private void onProgressChanged(object source, object obj)
        {
            int progress = (int)obj;
            ProgressVisible = true;
            InfoTextVisible = true;
            InfoText = "Loading tracks...";
            ProgressBarProgress = progress;
            Console.WriteLine("Progress = " + progress + "%");
            if (progress == 100)
            {
                ProgressVisible = false;
                InfoTextVisible = false;
                InfoText = "";
            }
                
            
        } 
    }
}
