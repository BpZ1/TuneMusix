using TuneMusix.Data;
using TuneMusix.Helpers;
using TuneMusix.Model;
using TuneMusix.Helpers.MediaPlayer;
using TuneMusix.Data.SQLDatabase;
using TuneMusix.Data.DataModelOb;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using System.Threading;
using System.ComponentModel;

namespace TuneMusix.ViewModel
{
    partial class ViewModelMain : ViewModelBase
    {
        public RelayCommand GetFiles { get; set; }
        public RelayCommand AddFolder { get; set; }
        public RelayCommand DeleteTracks { get; set; }
        public RelayCommand ExitApplication { get; set; }
        public RelayCommand DebugMethod { get; set; }  
        public RelayCommand OpenOptionsWindow { get; set; }
        public RelayCommand SaveData { get; set; }

        private DataModel dataModel = DataModel.Instance;
        private AudioControls audioControls = AudioControls.Instance;
        private Options options = Options.Instance;
        private SQLLoader loader;
        private BackgroundWorker loadingWorker;

        public static SnackbarMessageQueue MessageQueue { get; set; }

        //constructor
        public ViewModelMain()
        {
            //Load data from database.
            loader = new SQLLoader();
            loadingWorker = new BackgroundWorker();
            loadingWorker.DoWork += loader.LoadFromDB;
            loadingWorker.RunWorkerCompleted += OnLoadingComplete;
            loadingWorker.RunWorkerAsync();


            //notification
            MessageQueue = new SnackbarMessageQueue();
            dataModel.DataModelChanged += _onRootFoldersChanged;

            //Relaycommands
            GetFiles = new RelayCommand(_getFiles);
            AddFolder = new RelayCommand(_addFolder);
            DebugMethod = new RelayCommand(_debugMethod);
            ExitApplication = new RelayCommand(_exitApplication);
            SaveData = new RelayCommand(_saveData);
            OpenOptionsWindow = new RelayCommand(_openOptionsWindow);      
        }

        

        public static void Notification(string message)
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
