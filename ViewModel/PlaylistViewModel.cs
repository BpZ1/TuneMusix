using MaterialDesignThemes.Wpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers;
using TuneMusix.Model;
using TuneMusix.View.Dialog;
using TuneMusix.ViewModel.Dialog;

namespace TuneMusix.ViewModel
{
    class PlaylistViewModel : ViewModelListBase, IDragSource
    {

        DataModel dataModel = DataModel.Instance;

        public RelayCommand OpenDialog { get; set; }
        public RelayCommand SelectionChanged { get; set; }
        public RelayCommand PlaylistDropped { get; set; }
        public RelayCommand SelectPlaylist { get; set; }

        public ObservableCollection<Track> SelectedTracks { get; set; }
        private Playlist _selectedPlaylist;

        //constructor
        public PlaylistViewModel()
        {
            SelectedTracks = new ObservableCollection<Track>();
            SelectionChanged = new RelayCommand(selectionChanged);
            OpenDialog = new RelayCommand(openDialog);                    
            PlaylistDropped = new RelayCommand(playlistDropped);
            SelectPlaylist = new RelayCommand(selectPlaylist);

            //events
            dataModel.DataModelChanged += OnDataModelChanged;
            
        }

        private void selectPlaylist(object argument)
        {
            var playlist = argument as Playlist;
            if(playlist != null)
            {
                SelectedPlaylist = playlist;
            }
        }

        public Playlist SelectedPlaylist
        {
            get { return this._selectedPlaylist; }
            set
            {
                this._selectedPlaylist = value;
                Console.WriteLine("Selected Playlist: " + _selectedPlaylist.Name);
                RaisePropertyChanged("SelectedPlaylist");
            }
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
            //implement
        }
        

        private void playlistDropped(object argument)
        {
            Console.WriteLine("Dropped type:" + argument.GetType().ToString());
        }

        public void StartDrag(IDragInfo dragInfo)
        {
            Console.WriteLine("Can not Drag because selection is null");
            if (SelectedPlaylist != null)
            {
                dragInfo.Data = SelectedPlaylist;
                dragInfo.Effects = DragDropEffects.Copy;
                Console.WriteLine("Started dragging: " + dragInfo.Data.ToString());
            }
        }

        public bool CanStartDrag(IDragInfo dragInfo)
        {
            Console.WriteLine("Can I drag?");
            if (dragInfo != null)
            {
                Console.WriteLine("Can Drag");
                return true;
            }
            else
            {
                Console.WriteLine("Can not Drag");
                return false;
            }
        }

        public void Dropped(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }

        public void DragCancelled()
        {
            Console.WriteLine("Drag was Cancelled!");
        }

        public bool TryCatchOccurredException(Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
