﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using TuneMusix.Data;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        DataModel dataModel = DataModel.Instance;


        //Getter and setter
        public ObservableCollection<Track> TrackList
        {
            get { return dataModel.TrackList; }
        }

        //Getter and setter
        public Track CurrentTrackMVM
        {
            get { return dataModel.CurrentTrackDM; }
            set
            {
                dataModel.CurrentTrackDM = value;
                RaisePropertyChanged("CurrentTrackMVM");
            }
        }
        //Getter and setter
        public ObservableCollection<Playlist> Playlists
        {
            get { return dataModel.Playlists; }
            set
            {
                dataModel.Playlists = value;
                RaisePropertyChanged("Playlists");
            }
        }
        //Getter and setter
        public Playlist CurrentPlaylist
        {
            get { return dataModel.CurrentPlaylist; }
            set
            {
                dataModel.CurrentPlaylist = value;
                RaisePropertyChanged("CurrentPlaylist");
            }
        }

        //Getter and setter
        public Playlist SelectedPlaylist
        {
            get { return dataModel.SelectedPlaylist; }
            set
            {
                dataModel.SelectedPlaylist = value;
                RaisePropertyChanged("SelectedPlaylist");
            }
        }
        //Getter and setter
        public ObservableCollection<Track> SelectedTracks
        {
            get { return dataModel.SelectedTracks; }
            set
            {
                dataModel.SelectedTracks = value;
                RaisePropertyChanged("SelectedTracks");
            }
        }
        //basic ViewModelBase
        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        //Extra Stuff, shows why a base ViewModel is useful
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
