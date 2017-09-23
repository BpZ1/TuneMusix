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
    class PlaylistViewModel : ViewModelBase, IDragSource,IDropTarget
    {

        DataModel dataModel = DataModel.Instance;

        public RelayCommand OpenDialog { get; set; }
        public RelayCommand SelectionChanged { get; set; }
        public RelayCommand SelectPlaylist { get; set; }
        public RelayCommand SetPlaylistCurrent { get; set; }
        public RelayCommand DeletePlaylist { get; set; }

        public ObservableCollection<Track> SelectedTracks { get; set; }
        private bool _isDragging;

        private Playlist _selectedPlaylist;

        //constructor
        public PlaylistViewModel()
        {
            IsDragging = false;

            SelectedTracks = new ObservableCollection<Track>();
            OpenDialog = new RelayCommand(_openDialog);                    
            SelectPlaylist = new RelayCommand(_selectPlaylist);
            SetPlaylistCurrent = new RelayCommand(_setPlaylistCurrent);
            DeletePlaylist = new RelayCommand(_deletePlaylist);

            //events
            dataModel.DataModelChanged += OnDataModelChanged;
            
        }

        private void _selectPlaylist(object argument)
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
                RaisePropertyChanged("SelectedPlaylist");
            }
        }
        public bool IsDragging
        {
            get { return this._isDragging; }
            set
            {
                this._isDragging = value;
                RaisePropertyChanged("IsDragging");
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
        private async void _openDialog(object o)
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

        private void _setPlaylistCurrent(object argument)
        {
            if (SelectedPlaylist != null)
            {
                dataModel.CurrentPlaylist = SelectedPlaylist;
            }
        }

        private void _deletePlaylist(object argument)
        {
            if (SelectedPlaylist != null)
            {
                if (CurrentPlaylist != null)
                {
                    if (CurrentPlaylist == SelectedPlaylist)
                    {
                        CurrentPlaylist = null;
                    }
                }
                dataModel.Delete(SelectedPlaylist);
                SelectedPlaylist = null;
            }
        }

        /// <summary>
        /// Gets called when the drag is started and sets the dragging boolean.
        /// </summary>
        /// <param name="dragInfo"></param>
        public void StartDrag(IDragInfo dragInfo)
        {
            IsDragging = true;
            if (SelectedPlaylist != null)
            {
                dragInfo.Data = dragInfo.SourceItem;
                dragInfo.Effects = DragDropEffects.Copy;
            }
        }

        public bool CanStartDrag(IDragInfo dragInfo)
        {
            if (dragInfo != null)  return true;
            else  return false; 
        }

        public void DragCancelled()
        {
            IsDragging = false;
        }

        public bool TryCatchOccurredException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Dropped(IDropInfo dropInfo)
        {
            IsDragging = false;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.NotHandled = true;
            Playlist sourceItem = dropInfo.Data as Playlist;
            if (sourceItem != null)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }
        /// <summary>
        /// Is called if the drag ended over a valid dragtarget.
        /// </summary>
        /// <param name="dropInfo"></param>
        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.VisualTarget.GetType() == typeof(System.Windows.Controls.Button))
            {
                _deletePlaylist(null);
            }
        }
    }
}
