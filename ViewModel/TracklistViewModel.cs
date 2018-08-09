using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using TuneMusix.Helpers;
using TuneMusix.Helpers.Dialogs;
using TuneMusix.Helpers.Enums;
using TuneMusix.Model;
using TuneMusix.View.Dialog;
using TuneMusix.ViewModel.Dialog;

namespace TuneMusix.ViewModel
{
    class TracklistViewModel : ViewModelBase
    {

        public ObservableCollection<Track> SelectedTracks { get; set; }
        private string searchText;

        private const string DESCENDING_SORTED_ICON = "ChevronDown";
        private const string ASCENDING_SORTED_ICON = "ChevronUp";
        private HeaderType sortedColumn;
        private SortingType sortingType;
        private DelayIterativeSearch searchService;

        //Relaycommands
        public RelayCommand PlayTrack { get; set; }
        public RelayCommand DeleteSelectedTracks { get; set; }
        public RelayCommand AddToPlaylist { get; set; }
        public RelayCommand SelectionChanged { get; set; }
        public RelayCommand SearchChanged { get; set; }
        public RelayCommand DeleteSearch { get; set; }
        public RelayCommand ColumnClicked { get; set; }
        public RelayCommand TrackDoubleClicked { get; set; }
        public RelayCommand AddTracksToQueue { get; set; }
        public RelayCommand OpenNewPlaylistDialog { get; set; }

        private ObservableCollection<Track> filteredTracks;      

        //Constructor
        public TracklistViewModel()
        {
            searchText = "";
            sortingType = SortingType.Ascending;
            sortedColumn = HeaderType.Title;
            searchService = new DelayIterativeSearch(TrackList.ToArray());
            searchService.SearchTaskCompleted += onSearchCompleted;
            SelectedTracks = new ObservableCollection<Track>();
            filteredTracks = TrackList;
            queueSearchTask();

            #region commands
            DeleteSelectedTracks = new RelayCommand(deleteSelectedTracks);
            AddToPlaylist = new RelayCommand(addToPlaylist);
            SelectionChanged = new RelayCommand(selectionChanged);
            PlayTrack = new RelayCommand(playTrack);
            SearchChanged = new RelayCommand(searchChanged);
            DeleteSearch = new RelayCommand(deleteSearch);
            ColumnClicked = new RelayCommand(columnClicked);
            TrackDoubleClicked = new RelayCommand(trackDoubleClicked);
            AddTracksToQueue = new RelayCommand(addTracksToQueue);
            OpenNewPlaylistDialog = new RelayCommand(newPlaylistDialog);
            #endregion
            //events
            dataModel.DataModelChanged += OnTrackListChanged;
        }
        #region properties
        /// <summary>
        /// Filtered list of tracks.
        /// </summary>
        public ObservableCollection<Track> FilteredTracks
        {
            get { return filteredTracks; }
        }
        public string SelectedItemsText
        {
            get { return "Selected items: " + SelectedTracks.Count; }
        }
        /// <summary>
        /// Text contained in the search box.
        /// </summary>
        public string SearchBoxText
        {
            get;
            set;
        }
        #endregion
        #region commands
        /// <summary>
        /// Deletes all selected tracks from the tracklist.
        /// </summary>
        /// <param name="argument"></param>
        private void deleteSelectedTracks(object argument)
        {
            dataModel.Delete(SelectedTracks.ToList<Track>());
        }
        /// <summary>
        /// Adds all selected tracks to the playlist with the given id.
        /// </summary>
        /// <param name="argument"></param>
        private void addToPlaylist(object argument)
        {
            Playlist selectedPlaylist = argument as Playlist;

            if (selectedPlaylist != null)
            {
                dataModel.AddTracksToPlaylist(SelectedTracks.ToList<Track>(), selectedPlaylist);
            }
        }
        private void selectionChanged(object argument)
        {
            var listView = argument as ListView;
            if (listView == null) return;

            SelectedTracks.Clear();
            foreach (Track track in listView.SelectedItems)
            {
                SelectedTracks.Add(track);
            }
            RaisePropertyChanged("SelectedItemsText");
        }
        private void playTrack(object argument)
        {
            if (SelectedTracks == null) return;
            if (SelectedTracks.Count > 0)
            {
                CurrentPlaylist = null;
                TrackQueue = SelectedTracks.ToList<Track>();
            }
        }
        /// <summary>
        /// Called every time the text in the textbox changes
        /// </summary>
        /// <param name="argument"></param>
        private void searchChanged(object argument)
        {
            if (((string)argument).Equals(searchText))
                return;
            
            searchText = (string)argument;         
            queueSearchTask();                       
        }
        /// <summary>
        /// Changes the type of the column to sort.
        /// Changes the sorting type if the same column is
        /// clicked that is already sorted.
        /// </summary>
        /// <param name="argument"></param>
        private void columnClicked(object argument)
        {
            var columnType = argument as string;
            HeaderType previousType = sortedColumn;
            switch (columnType)
            {
                case "TitleColumn":
                    sortedColumn = HeaderType.Title;
                    break;

                case "InterpretColumn":
                    sortedColumn = HeaderType.Interpret;
                    break;

                case "AlbumColumn":
                    sortedColumn = HeaderType.Album;
                    break;

                case "YearColumn":
                    sortedColumn = HeaderType.Year;
                    break;

                case "GenreColumn":
                    sortedColumn = HeaderType.Genre;
                    break;

                case "RatingColumn":
                    sortedColumn = HeaderType.Rating;
                    break;

                case "DurationColumn":
                    sortedColumn = HeaderType.Duration;
                    break;
            }
            //If column did not change change sorting mode.
            if (previousType == sortedColumn)
            {
                if (sortingType == SortingType.Ascending)
                {
                    sortingType = SortingType.Descending;
                }
                else
                {
                    sortingType = SortingType.Ascending;
                }
                RaisePropertyChanged("SortingIcon");
            }
            RaisePropertyChanged("TitleSorted");
            RaisePropertyChanged("InterpretSorted");
            RaisePropertyChanged("AlbumSorted");
            RaisePropertyChanged("YearSorted");
            RaisePropertyChanged("GenreSorted");
            RaisePropertyChanged("RatingSorted");
            RaisePropertyChanged("DurationSorted");
            //start sorting
            sortListView();
            RaisePropertyChanged("FilteredTracks");
        }
        private void trackDoubleClicked(object argument)
        {
            if (argument == null)
                throw new ArgumentNullException();

            var track = argument as Track;
            if (track != null)
            {
                CurrentTrack = track;
                dataModel.CurrentPlaylist = null;
                dataModel.TrackQueue = new ObservableCollection<Track>(new List<Track>(){track});
                dataModel.QueueIndex = dataModel.TrackQueue.IndexOf(track);
            }
        }
        private void addTracksToQueue(object argument)
        {
            if (SelectedTracks == null) return;
            if (SelectedTracks.Count > 0)
            {
                CurrentPlaylist = null;
                foreach(Track track in SelectedTracks)
                {
                    if(!dataModel.TrackQueue.Contains(track))
                        dataModel.TrackQueue.Add(track);
                }
                RaisePropertyChanged("TrackQueue");
            }
        }
        #region playlist creation
        /// <summary>
        /// Open the dialog for adding a playlist.
        /// </summary>
        /// <param name="argument"></param>
        private async void newPlaylistDialog(object argument)
        {
            var view = new GetTextDialog
            {
                DataContext = new GetTextViewModel("New playlist")               
            };
            //show the dialog
            var result = await DialogHost.Show(view, "DialogHost", OpenedEventHandler, playlistDialogClosingHandler);
        }
        /// <summary>
        /// Intercept the open and affect the dialog using eventArgs.Session.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventargs"></param>
        private void OpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
        }

        /// <summary>
        /// Closes the dialog for adding a playlist and adds the playlist if accept was selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void playlistDialogClosingHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false) return;

            //chancel the closing
            eventArgs.Cancel();
            var content = eventArgs.Session.Content as GetTextDialog;
            eventArgs.Session.UpdateContent(new GetTextDialog());
            var dataContext = content.DataContext as GetTextViewModel;
            if (dataContext != null)
            {
                if (dataContext.TextBoxText.Count<char>() >= 2)
                {
                    Playlist playlist = dataModel.AddPlaylist(dataContext.TextBoxText);
                    if(playlist != null)
                    {
                        dataModel.AddTracksToPlaylist(SelectedTracks.ToList(), playlist);
                    }             
                }
                else
                {
                    DialogService.NotificationMessage("The name has to be longer than two characters.");
                }
            }
            Task.Factory.StartNew(() => { }).ContinueWith((t, _) => eventArgs.Session.Close(false), null,
                         TaskScheduler.FromCurrentSynchronizationContext());

        }
        #endregion
        #endregion

        #region methods
        private void OnTrackListChanged(object source,object obj)
        {
            filteredTracks = TrackList;
            searchService.Itemlist = TrackList.ToArray();
            queueSearchTask();
        }
        private void sortListView()
        {
            Logger.Log("Sorting " + sortedColumn);

            IEnumerable<Track> sortedList = null;

            if (sortedColumn == HeaderType.Album)
            {
                if(sortingType == SortingType.Ascending)
                {
                    sortedList =
                       from track in FilteredTracks
                       orderby track.Album ascending
                       select track;
                }
                else
                {
                    sortedList =
                       from track in FilteredTracks
                       orderby track.Album descending
                       select track;
                }
            }
            if (sortedColumn == HeaderType.Duration)
            {
                if (sortingType == SortingType.Ascending)
                {
                    sortedList =
                       from track in FilteredTracks
                       orderby track.Duration ascending
                       select track;
                }
                else
                {
                    sortedList =
                       from track in FilteredTracks
                       orderby track.Duration descending
                       select track;
                }
            }
            if (sortedColumn == HeaderType.Genre)
            {
                if (sortingType == SortingType.Ascending)
                {
                    sortedList =
                       from track in FilteredTracks
                       orderby track.Genre ascending
                       select track;
                }
                else
                {
                    sortedList =
                       from track in FilteredTracks
                       orderby track.Genre descending
                       select track;
                }
            }
            if (sortedColumn == HeaderType.Interpret)
            {
                if (sortingType == SortingType.Ascending)
                {
                    sortedList =
                       from track in FilteredTracks
                       orderby track.Interpret ascending
                       select track;
                }
                else
                {
                    sortedList =
                       from track in FilteredTracks
                       orderby track.Interpret descending
                       select track;
                }
            }
            if (sortedColumn == HeaderType.Rating)
            {
                if (sortingType == SortingType.Ascending)
                {
                    sortedList =
                       from track in FilteredTracks
                       orderby track.Rating ascending
                       select track;
                }
                else
                {
                    sortedList =
                       from track in FilteredTracks
                       orderby track.Rating descending
                       select track;
                }
            }
            if (sortedColumn == HeaderType.Title)
            {
                if (sortingType == SortingType.Ascending)
                {
                    sortedList =
                       from track in FilteredTracks
                       orderby track.Title ascending
                       select track;
                }
                else
                {
                    sortedList =
                       from track in FilteredTracks
                       orderby track.Title descending
                       select track;
                }
            }
            if (sortedColumn == HeaderType.Year)
            {
                if (sortingType == SortingType.Ascending)
                {
                    sortedList =
                       from track in FilteredTracks
                       orderby track.Year ascending
                       select track;
                }
                else
                {
                    sortedList =
                       from track in FilteredTracks
                       orderby track.Year descending
                       select track;
                }
            }
            if (sortedList != null)
                filteredTracks = new ObservableCollection<Track>(sortedList);
        }
        #region searching methods
        /// <summary>
        /// Deletes the search text and updates the list.
        /// </summary>
        /// <param name="argument"></param>
        private void deleteSearch(object argument)
        {
            if (searchText.Equals(""))
                return;

            SearchBoxText = "";          
            RaisePropertyChanged("SearchBoxText");
            searchText = "";
            queueSearchTask();  
        }
        private void onSearchCompleted(object result)
        {
            filteredTracks = new ObservableCollection<Track>((List<Track>)result);
            sortListView();
            RaisePropertyChanged("FilteredTracks");
            RaisePropertyChanged("TrackDurationSum");
        }
        /// <summary>
        /// Queues a search task and sorts and updates the view on completion.
        /// </summary>
        private void queueSearchTask()
        {
            searchService.QueueSearchTask(searchText);
        }
        #endregion
        #endregion
        /// <summary>
        /// Defines the arrow icon type besides the header in the columns.
        /// </summary>
        public string SortingIcon
        {
            get
            {
                if (sortingType == SortingType.Ascending)
                {
                    return ASCENDING_SORTED_ICON;
                }
                else
                {
                    return DESCENDING_SORTED_ICON;
                }               
            }
        }
        public bool TitleSorted
        {
            get
            {
                if (sortedColumn == HeaderType.Title)
                    return true;

                return false;
            }
        }
        public bool InterpretSorted
        {
            get
            {
                if (sortedColumn == HeaderType.Interpret)
                    return true;

                return false;
            }
        }
        public bool AlbumSorted
        {
            get
            {
                if (sortedColumn == HeaderType.Album)
                    return true;

                return false;
            }
        }
        public bool YearSorted
        {
            get
            {
                if (sortedColumn == HeaderType.Year)
                    return true;

                return false;
            }
        }
        public bool GenreSorted
        {
            get
            {
                if (sortedColumn == HeaderType.Genre)
                    return true;

                return false;
            }
        }
        public bool RatingSorted
        {
            get
            {
                if (sortedColumn == HeaderType.Rating)
                    return true;

                return false;
            }
        }
        public bool DurationSorted
        {
            get
            {
                if (sortedColumn == HeaderType.Duration)
                    return true;

                return false;
            }
        }

        public string TrackDurationSum
        {
            get
            {
                string result = "Total playtime: [";
                result += sumAllTracks();
                return result + "]";
            }
        }
        private string sumAllTracks()
        {
            string returnValue = "";
            foreach(Track track in filteredTracks)
            {
                returnValue = TrackService.AddDurations(returnValue, track.Duration);
            }
            return returnValue;
        }
    }
}
