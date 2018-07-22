using TuneMusix.Helpers;
using TuneMusix.Model;
using TuneMusix.Helpers.MediaPlayer;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using TuneMusix.Helpers.Interface;

namespace TuneMusix.ViewModel
{
    partial class ViewModelMain : ViewModelBase
    {
        public RelayCommand GetFiles { get; set; }
        public RelayCommand AddFolder { get; set; }
        public RelayCommand DeleteTracks { get; set; }
        public RelayCommand ExitApplication { get; set; }
        public RelayCommand ExitButtonPressed { get; set; }
        public RelayCommand DebugMethod { get; set; }  
        public RelayCommand OpenOptionsWindow { get; set; }
        public RelayCommand SaveData { get; set; }

        private AudioControls audioControls = AudioControls.Instance;
        private Options options = Options.Instance;
        private LoadingBarManager loadingBarManager = LoadingBarManager.Instance;

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
            loadingBarManager.ProgressChanged += onProgressChanged;
            loadingBarManager.LoadingStarted += onLoadingStarted;
            loadingBarManager.LoadingFinished += onLoadingFinished;
            loadingBarManager.InfoTextChanged += onInfoTextChanged;

            //Relaycommands
            GetFiles = new RelayCommand(getFiles);
            AddFolder = new RelayCommand(addFolder);
            ExitApplication = new RelayCommand(exitApplication);
            ExitButtonPressed = new RelayCommand(exitButtonPressed);
            SaveData = new RelayCommand(saveData);
            OpenOptionsWindow = new RelayCommand(openOptionsWindow);
            DebugMethod = new RelayCommand(debugMethod);
        }

        /// <summary>
        /// Displays a given message on the bottom screen for a few seconds.
        /// </summary>
        /// <param name="message"></param>
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
