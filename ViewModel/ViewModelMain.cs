using TuneMusix.Data;
using TuneMusix.Helpers;
using TuneMusix.Model;
using TuneMusix.Helpers.MediaPlayer;
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
        public RelayCommand OpenOptionsWindow { get; set; }
        public RelayCommand SaveData { get; set; }

        private DataModel dataModel = DataModel.Instance;
        private AudioControls audioControls = AudioControls.Instance;
        private Options options = Options.Instance;

        private bool infoTextVisible = false;
        private bool progressVisible = false;
        private int progress;
        private string infoText;

        public static SnackbarMessageQueue MessageQueue { get; set; }

        //constructor
        public ViewModelMain()
        {
            //Startup loading
            dataModel.DatabaseStartupLoading();
            
            //notification
            MessageQueue = new SnackbarMessageQueue();
            dataModel.DataModelChanged += onRootFoldersChanged;
            dataModel.ProgressChanged += onProgressChanged;
            dataModel.LoadingStarted += onLoadingStarted;
            dataModel.LoadingFinished += onLoadingFinished;

            //Relaycommands
            GetFiles = new RelayCommand(getFiles);
            AddFolder = new RelayCommand(addFolder);
            DebugMethod = new RelayCommand(debugMethod);
            ExitApplication = new RelayCommand(exitApplication);
            SaveData = new RelayCommand(saveData);
            OpenOptionsWindow = new RelayCommand(openOptionsWindow);      
        }


        public static void Notification(string message)
        {
            //the message queue can be called from any thread
            Task.Factory.StartNew(() => MessageQueue.Enqueue(message));
        }


        #region getter and setter 
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
        //visibility state of the progress bar and its text
        public bool ProgressVisible
        {
            get { return progressVisible; }
            set
            {
                progressVisible = value;
                RaisePropertyChanged("ProgressVisible");
            }
        }
        //visiblity state of the info text
        public bool InfoTextVisible
        {
            get { return infoTextVisible; }
            set
            {
                infoTextVisible = value;
                RaisePropertyChanged("InfoTextVisible");
            }
        }
        //progress bar that shows the loading progress
        public int ProgressBarProgress
        {
            get { return progress; }
            set
            {
                progress = value;
                RaisePropertyChanged("ProgressBarProgress");
                RaisePropertyChanged("ProgressBarText");
            }
        }
        //Text displayed on the progress bar
        public string ProgressBarText
        {
            get { return (progress + "%"); }
        }
        //Info text shown besides the loading bar
        public string InfoText
        {
            get { return infoText; }
            set
            {
                infoText = value;
                RaisePropertyChanged("InfoText");
            }
        }
        #endregion
    }
}
