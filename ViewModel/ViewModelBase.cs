using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers.Util;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {

        protected DataModel dataModel = DataModel.Instance;

        public ObservableCollection<Track> TrackList
        {
            get { return dataModel.TrackList; }
        }


        public Track CurrentTrack
        {
            get { return dataModel.CurrentTrack; }
            set
            {
                dataModel.CurrentTrack = value;
                RaisePropertyChanged("CurrentTrack");
            }
        }

        public ObservableList<Playlist> Playlists
        {
            get { return dataModel.Playlists; }
            set
            {
                dataModel.Playlists = value;
                RaisePropertyChanged("Playlists");
            }
        }

        public Playlist CurrentPlaylist
        {
            get { return dataModel.CurrentPlaylist; }
            set
            {
                dataModel.CurrentPlaylist = value;
                RaisePropertyChanged("CurrentPlaylist");
            }
        }

        public ObservableCollection<Folder> RootFolders
        {
            get { return dataModel.RootFolders; }
        }
        public List<Track> TrackQueue
        {
            get { return dataModel.TrackQueue.ToList<Track>(); }
            set { dataModel.TrackQueue = new ObservableList<Track>(value); }
        }
        public int TrackQueueIndex
        {
            get { return dataModel.QueueIndex; }
            set { dataModel.QueueIndex = value; }
        }
        
        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        bool? _CloseWindowFlag;
        public bool? CloseWindowFlag
        {
            get { return _CloseWindowFlag; }
            set
            {
                _CloseWindowFlag = value;
                RaisePropertyChanged("CloseWindowFlag");
            }
        }

        public virtual void CloseWindow(bool? result = true)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                CloseWindowFlag = CloseWindowFlag == null
                    ? true
                    : !CloseWindowFlag;
            }));
        }
    }
}
