using System;
using System.ComponentModel;

namespace TuneMusix.ViewModel
{
    partial class ViewModelMain
    {

        private void _onRootFoldersChanged(object source,object obj)
        {
             RaisePropertyChanged("TrackList");
        }

        private void OnLoadingComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            audioControls.LoadEffects();
        }

    }
}
