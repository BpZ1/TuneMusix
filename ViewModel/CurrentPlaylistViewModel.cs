using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{
    class CurrentPlaylistViewModel : ViewModelBase, IDropTarget
    {

        DataModel dataModel = DataModel.Instance;
      
        public RelayCommand SelectionChanged { get; set; }
        public RelayCommand SetPlaylistIndex { get; set; }
        public RelayCommand RemoveFromPlaylist { get; set; }
        public RelayCommand OnItemDrop { get; set; }
        public ObservableCollection<Track> SelectedTracks { get; set; }


        public CurrentPlaylistViewModel()
        {

            SelectedTracks = new ObservableCollection<Track>();
            SelectionChanged = new RelayCommand(selectionChangedCurrentPlaylist);
            SetPlaylistIndex = new RelayCommand(newIndex);
            RemoveFromPlaylist = new RelayCommand(removeFromCurrentPlaylist);

            dataModel.CurrentPlaylistChanged += OnCurrentPlaylistChanged;
        }


        private void selectionChangedCurrentPlaylist(object argument)
        {
            var listView = argument as ListView;
            if (listView != null)
            {
                SelectedTracks.Clear();
                foreach (Track track in listView.SelectedItems)
                {
                    SelectedTracks.Add(track);
                }
            }
        }

        private void removeFromCurrentPlaylist(object argument)
        {
            if (SelectedTracks != null && dataModel.CurrentPlaylist != null)
            {
                dataModel.RemoveTracksFromPlaylist(SelectedTracks.ToList<Track>(), dataModel.CurrentPlaylist);
            }
        }

        private void newIndex(object argument)
        {
            if (SelectedTracks != null && CurrentPlaylist != null)
            {
                var track = SelectedTracks.First();
                dataModel.CurrentTrack = track;
                dataModel.QueueIndex = dataModel.CurrentPlaylist.Tracklist.IndexOf(track);
            }
        }

        private void OnCurrentPlaylistChanged(object source, object newPlaylist)
        {
            RaisePropertyChanged("CurrentPlaylistName");
            RaisePropertyChanged("CurrentPlaylistTracks");
            SelectedTracks.Clear();
        }


        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.NotHandled = true;
            Playlist sourceItem = dropInfo.Data as Playlist;
            if (sourceItem != null)
            {
                Console.WriteLine("Dragging: " + sourceItem.Name );
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            Console.WriteLine("DROPPED");
            Playlist playlist = dropInfo.Data as Playlist;
            dataModel.CurrentPlaylist = playlist;
        }

        public string CurrentPlaylistName
        {
            get
            {
                if(dataModel.CurrentPlaylist != null)
                {
                    return dataModel.CurrentPlaylist.Name;
                }
                else
                {
                    return "...";
                }
            }
        }

        public ObservableCollection<Track> CurrentPlaylistTracks
        {
            get
            {
                if (dataModel.CurrentPlaylist != null)
                {
                    return dataModel.CurrentPlaylist.Tracklist;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
