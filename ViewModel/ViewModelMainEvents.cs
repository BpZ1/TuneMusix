using System.ComponentModel;

namespace TuneMusix.ViewModel
{
    partial class ViewModelMain
    {

        private void onRootFoldersChanged(object source,object obj)
        {
             RaisePropertyChanged("TrackList");
        }

        //Loading of tracks is finished
        private void onLoadingComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            audioControls.LoadEffects();
        }

        private void onProgressChanged(object source, object obj)
        {
            int progress = (int)obj;
            ProgressBarProgress = progress;             
        } 

        private void onLoadingStarted(object sender, object obj)
        {
            ProgressVisible = true;
            InfoTextVisible = true;
            InfoText = "Loading tracks...";
        }
        private void onLoadingFinished(object sender, object obj)
        {
            ProgressVisible = false;
            InfoTextVisible = false;
            InfoText = "";
        }
    }
}
