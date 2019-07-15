
namespace TuneMusix.ViewModel
{
    partial class ViewModelMain
    {

        private void OnRootFoldersChanged(object source,object obj)
        {
             RaisePropertyChanged("TrackList");
        }

        private void OnProgressChanged(object obj)
        {
            int progress = (int)obj;
            ProgressBarProgress = progress;             
        } 

        private void OnLoadingStarted(object obj)
        {
            ProgressVisible = true;
            InfoTextVisible = true;
            InfoText = "Loading tracks...";
        }

        private void OnInfoTextChanged(object obj)
        {
            var message = obj as string;
            InfoText = message;
        }

        private void OnLoadingFinished(object obj)
        {
            ProgressVisible = false;
            InfoTextVisible = false;
            InfoText = "";
        }
    }
}
