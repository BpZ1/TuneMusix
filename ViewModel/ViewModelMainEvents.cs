
namespace TuneMusix.ViewModel
{
    partial class ViewModelMain
    {

        private void onRootFoldersChanged(object source,object obj)
        {
             RaisePropertyChanged("TrackList");
        }

        private void onProgressChanged(object obj)
        {
            int progress = (int)obj;
            ProgressBarProgress = progress;             
        } 

        private void onLoadingStarted(object obj)
        {
            ProgressVisible = true;
            InfoTextVisible = true;
            InfoText = "Loading tracks...";
        }

        private void onInfoTextChanged(object obj)
        {
            var message = obj as string;
            InfoText = message;
        }

        private void onLoadingFinished(object obj)
        {
            ProgressVisible = false;
            InfoTextVisible = false;
            InfoText = "";
        }
    }
}
