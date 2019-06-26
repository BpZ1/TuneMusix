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
        public RelayCommand OpenOptionsWindow { get; set; }
        public RelayCommand SaveData { get; set; }
        public static SnackbarMessageQueue MessageQueue { get; set; }

        private AudioControls audioControls = AudioControls.Instance;
        private Options _options = Options.Instance;
        private LoadingBarManager _loadingBarManager = LoadingBarManager.Instance;

        private bool _infoTextVisible = false;
        private bool _progressVisible = false;
        private int _progress;
        private string _infoText;


        //constructor
        public ViewModelMain()
        {
            //Startup loading
            _dataModel.DatabaseStartupLoading();
            
            //notification
            MessageQueue = new SnackbarMessageQueue();
            _dataModel.DataModelChanged += OnRootFoldersChanged;
            _loadingBarManager.ProgressChanged += OnProgressChanged;
            _loadingBarManager.LoadingStarted += OnLoadingStarted;
            _loadingBarManager.LoadingFinished += OnLoadingFinished;
            _loadingBarManager.InfoTextChanged += OnInfoTextChanged;

            //Relaycommands
            GetFiles = new RelayCommand(_getFiles);
            AddFolder = new RelayCommand(_addFolder);
            ExitApplication = new RelayCommand(_exitApplication);
            ExitButtonPressed = new RelayCommand(_exitButtonPressed);
            SaveData = new RelayCommand(_saveData);
            OpenOptionsWindow = new RelayCommand(_openOptionsWindow);
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
            get { return _progressVisible; }
            set
            {
                _progressVisible = value;
                RaisePropertyChanged("ProgressVisible");
            }
        }
        //visiblity state of the info text
        public bool InfoTextVisible
        {
            get { return _infoTextVisible; }
            set
            {
                _infoTextVisible = value;
                RaisePropertyChanged("InfoTextVisible");
            }
        }
        //progress bar that shows the loading progress
        public int ProgressBarProgress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                RaisePropertyChanged("ProgressBarProgress");
                RaisePropertyChanged("ProgressBarText");
            }
        }
        //Text displayed on the progress bar
        public string ProgressBarText
        {
            get { return (_progress + "%"); }
        }
        //Info text shown besides the loading bar
        public string InfoText
        {
            get { return _infoText; }
            set
            {
                _infoText = value;
                RaisePropertyChanged("InfoText");
            }
        }
        #endregion
    }
}
