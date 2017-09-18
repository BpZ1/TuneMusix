using System;
using System.Collections.Generic;
using System.Linq;
using TuneMusix.Data;
using TuneMusix.Helpers;
using TuneMusix.Model;
using WinForms = System.Windows.Forms;
using System.Timers;
using TuneMusix.Helpers.MediaPlayer;

//TODO: Multithreading for dataloading, improve performance (binary search when list sorted)

namespace TuneMusix.ViewModel
{
    partial class ViewModelMain : ViewModelBase
    {

        public RelayCommand GetFiles { get; set; }
        public RelayCommand AddFolder { get; set; }
        public RelayCommand AddToPlaylist { get; set; }
        public RelayCommand DeleteTracks { get; set; }
        public RelayCommand ExitApplication { get; set; }
        public RelayCommand DebugMethod { get; set; }  

        DataModel dataModel = DataModel.Instance;
        AudioControls audioControls = AudioControls.Instance;
        Options options = Options.Instance;

  
        //constructor
        public ViewModelMain()
        {
            //Load data from database.
            LoadFromDB();

            dataModel.DataModelChanged += RootFoldersChanged;

            //Relaycommands
            GetFiles = new RelayCommand(_getFiles);
            AddFolder = new RelayCommand(_addFolder);
            AddToPlaylist = new RelayCommand(_addToPlaylist);
            DeleteTracks = new RelayCommand(_deleteTracks);
            DebugMethod = new RelayCommand(_debugMethod);
            ExitApplication = new RelayCommand(_exitApplication);       
        }

        //Getter and setter    
        public string CurrentPlaylistName
        {
            get
            {
                if (CurrentPlaylist != null)
                {
                    return CurrentPlaylist.Name;
                }
                else
                {
                    return "...";
                }
            }
        }
        public bool TrackLoaded
        {
            get
            {
                return audioControls.TrackLoaded;
            }
        }
    }
}
