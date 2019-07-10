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

        protected DataModel _dataModel = DataModel.Instance;

        public ObservableList<Track> TrackList
        {
            get { return _dataModel.TrackList; }
        }


        public Track CurrentTrack
        {
            get { return _dataModel.CurrentTrack; }
            set
            {
                _dataModel.CurrentTrack = value;
                RaisePropertyChanged("CurrentTrack");
            }
        }

        public ObservableList<Playlist> Playlists
        {
            get { return _dataModel.Playlists; }
            set
            {
                _dataModel.Playlists = value;
                RaisePropertyChanged("Playlists");
            }
        }

        public Playlist CurrentPlaylist
        {
            get { return _dataModel.CurrentPlaylist; }
            set
            {
                _dataModel.CurrentPlaylist = value;
                RaisePropertyChanged("CurrentPlaylist");
            }
        }

        public ObservableCollection<Folder> RootFolders
        {
            get { return _dataModel.RootFolders; }
        }
        public ObservableList<Track> TrackQueue
        {
            get { return _dataModel.TrackQueue; }
            set { _dataModel.TrackQueue = value; }
        }
        public int TrackQueueIndex
        {
            get { return _dataModel.QueueIndex; }
            set { _dataModel.QueueIndex = value; }
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
