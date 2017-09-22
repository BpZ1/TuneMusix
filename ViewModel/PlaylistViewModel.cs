using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers;
using TuneMusix.Model;
using TuneMusix.View.Dialog;
using TuneMusix.ViewModel.Dialog;

namespace TuneMusix.ViewModel
{
    class PlaylistViewModel : ViewModelListBase
    {

        DataModel dataModel = DataModel.Instance;

        public RelayCommand OpenDialog { get; set; }
        public RelayCommand SelectionChanged { get; set; }
        public RelayCommand RemoveFromPlaylist { get; set; }
        public RelayCommand PlayTrack { get; set; }

        //constructor
        public PlaylistViewModel()
        {
            SelectionChanged = new RelayCommand(selectionChanged);
            OpenDialog = new RelayCommand(openDialog);
            RemoveFromPlaylist = new RelayCommand(removeFromPlaylist);
            PlayTrack = new RelayCommand(playTrack);
    
        //events
        dataModel.DataModelChanged += OnDataModelChanged;
        }

        private void OnDataModelChanged(object source,object changed)
        {
            RaisePropertyChanged("Playlists");
        }

        /// <summary>
        /// Open the dialog for adding a playlist.
        /// </summary>
        /// <param name="o"></param>
        private async void openDialog(object o)
        {
            var view = new AddPlaylistDialog
            {
                DataContext = new AddPlaylistViewModel()
            };

            //show the dialog
            var result = await DialogHost.Show(view, "DialogHost", OpenedEventHandler, ClosingEventHandler);

            //check the result...
            //Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }
        /// <summary>
        /// Intercept the open and affect the dialog using eventArgs.Session.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventargs"></param>
        private void OpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
        }

        /// <summary>
        /// Closes the dialog for adding a playlist and adds the playlist if accept was selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false) return;

            //OK, lets cancel the close...
            eventArgs.Cancel();
            Console.WriteLine("Before added");
            var content = eventArgs.Session.Content as AddPlaylistDialog;
            eventArgs.Session.UpdateContent(new AddPlaylistDialog());
            var dataContext = content.DataContext as AddPlaylistViewModel;
            if (dataContext != null)
            {
                dataModel.AddPlaylist(dataContext.TextBoxText);
            }
            Console.WriteLine("After added");
            Task.Factory.StartNew(()=>{}).ContinueWith((t, _) => eventArgs.Session.Close(false), null,
                         TaskScheduler.FromCurrentSynchronizationContext());
            Console.WriteLine("closed");

        }

        private void selectionChanged(object argument)
        {
            Console.WriteLine("Test3");
        }

        public void removeFromPlaylist(object argument)
        {
            Console.WriteLine("Test1");
        }

        public void playTrack(object argument)
        {
            Console.WriteLine("Test2");
        }
    }
}
