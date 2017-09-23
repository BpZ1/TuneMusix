﻿using TuneMusix.Data;
using TuneMusix.Helpers;
using TuneMusix.Model;
using TuneMusix.Helpers.MediaPlayer;
using TuneMusix.Data.SQLDatabase;
using TuneMusix.Data.DataModelOb;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;

namespace TuneMusix.ViewModel
{
    partial class ViewModelMain : ViewModelBase
    {
        public RelayCommand GetFiles { get; set; }
        public RelayCommand AddFolder { get; set; }
        public RelayCommand DeleteTracks { get; set; }
        public RelayCommand ExitApplication { get; set; }
        public RelayCommand DebugMethod { get; set; }  

        DataModel dataModel = DataModel.Instance;
        AudioControls audioControls = AudioControls.Instance;
        Options options = Options.Instance;
        SQLLoader loader;

        public static SnackbarMessageQueue MessageQueue { get; set; }

        //constructor
        public ViewModelMain()
        {
            //Load data from database.
            loader = new SQLLoader();
            loader.LoadFromDB();

            MessageQueue = new SnackbarMessageQueue();
            dataModel.DataModelChanged += RootFoldersChanged;

            //Relaycommands
            GetFiles = new RelayCommand(_getFiles);
            AddFolder = new RelayCommand(_addFolder);
            DebugMethod = new RelayCommand(_debugMethod);
            ExitApplication = new RelayCommand(_exitApplication);       
        }


        private static void Notification(string message)
        {
            //the message queue can be called from any thread
            Task.Factory.StartNew(() => MessageQueue.Enqueue(message));
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
