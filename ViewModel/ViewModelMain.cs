using TuneMusix.Data;
using TuneMusix.Helpers;
using TuneMusix.Model;
using TuneMusix.Helpers.MediaPlayer;
using TuneMusix.Data.SQLDatabase;

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

        //constructor
        public ViewModelMain()
        {
            //Load data from database.
            loader = new SQLLoader();
            loader.LoadFromDB();

            dataModel.DataModelChanged += RootFoldersChanged;

            //Relaycommands
            GetFiles = new RelayCommand(getFiles);
            AddFolder = new RelayCommand(addFolder);
            DeleteTracks = new RelayCommand(deleteTracks);
            DebugMethod = new RelayCommand(debugMethod);
            ExitApplication = new RelayCommand(exitApplication);       
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
