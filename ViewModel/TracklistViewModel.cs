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
using TuneMusix.Helpers.Util;
using TuneMusix.Model;
using TuneMusix.View.Dialog;
using TuneMusix.ViewModel.Dialog;

namespace TuneMusix.ViewModel
{
    class TracklistViewModel : ViewModelBase
    {
        public ObservableCollection<Track> SelectedTracks { get; set; }
        private string _searchText;

        private const string DESCENDING_SORTED_ICON = "ChevronDown";
        private const string ASCENDING_SORTED_ICON = "ChevronUp";
        private HeaderType _sortedColumn;
        private SortingType _sortingType;
        private DelayIterativeSearch _searchService;

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

        private ObservableCollection<Track> _filteredTracks;      

        //Constructor
        public TracklistViewModel()
        {
            _searchText = "";
            _sortingType = SortingType.Ascending;
            _sortedColumn = HeaderType.Title;
            _searchService = new DelayIterativeSearch(TrackList.ToArray());
            _searchService.SearchTaskCompleted += OnSearchCompleted;
            SelectedTracks = new ObservableCollection<Track>();
            _filteredTracks = TrackList;
            queueSearchTask();

            #region commands
            DeleteSelectedTracks = new RelayCommand(_deleteSelectedTracks);
            AddToPlaylist = new RelayCommand(_addToPlaylist);
            SelectionChanged = new RelayCommand(_selectionChanged);
            PlayTrack = new RelayCommand(_playTrack);
            SearchChanged = new RelayCommand(_searchChanged);
            DeleteSearch = new RelayCommand(_deleteSearch);
            ColumnClicked = new RelayCommand(_columnClicked);
            TrackDoubleClicked = new RelayCommand(_trackDoubleClicked);
            AddTracksToQueue = new RelayCommand(_addTracksToQueue);
            OpenNewPlaylistDialog = new RelayCommand(_newPlaylistDialog);
            #endregion
            //events
            _dataModel.DataModelChanged += OnTrackListChanged;
        }
        #region properties
        /// <summary>
        /// Filtered list of tracks.
        /// </summary>
        public ObservableCollection<Track> FilteredTracks
        {
            get { return _filteredTracks; }
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
        private void _deleteSelectedTracks(object argument)
        {
            _dataModel.Delete(SelectedTracks.ToList<Track>());
        }
        /// <summary>
        /// Adds all selected tracks to the playlist with the given id.
        /// </summary>
        /// <param name="argument"></param>
        private void _addToPlaylist(object argument)
        {
            Playlist selectedPlaylist = argument as Playlist;

            if (selectedPlaylist != null)
            {
                _dataModel.AddTracksToPlaylist(SelectedTracks.ToList<Track>(), selectedPlaylist);
            }
        }
        private void _selectionChanged(object argument)
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
        private void _playTrack(object argument)
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
        private void _searchChanged(object argument)
        {
            if (((string)argument).Equals(_searchText))
                return;
            
            _searchText = (string)argument;         
            queueSearchTask();                       
        }
        /// <summary>
        /// Changes the type of the column to sort.
        /// Changes the sorting type if the same column is
        /// clicked that is already sorted.
        /// </summary>
        /// <param name="argument"></param>
        private void _columnClicked(object argument)
        {
            var columnType = argument as string;
            HeaderType previousType = _sortedColumn;
            switch (columnType)
            {
                case "TitleColumn":
                    _sortedColumn = HeaderType.Title;
                    break;

                case "InterpretColumn":
                    _sortedColumn = HeaderType.Interpret;
                    break;

                case "AlbumColumn":
                    _sortedColumn = HeaderType.Album;
                    break;

                case "YearColumn":
                    _sortedColumn = HeaderType.Year;
                    break;

                case "GenreColumn":
                    _sortedColumn = HeaderType.Genre;
                    break;

                case "RatingColumn":
                    _sortedColumn = HeaderType.Rating;
                    break;

                case "DurationColumn":
                    _sortedColumn = HeaderType.Duration;
                    break;
            }
            //If column did not change change sorting mode.
            if (previousType == _sortedColumn)
            {
                if (_sortingType == SortingType.Ascending)
                {
                    _sortingType = SortingType.Descending;
                }
                else
                {
                    _sortingType = SortingType.Ascending;
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
        private void _trackDoubleClicked(object argument)
        {
            if (argument == null)
                throw new ArgumentNullException();

            var track = argument as Track;
            if (track != null)
            {
                CurrentTrack = track;
                _dataModel.CurrentPlaylist = null;
                _dataModel.TrackQueue = new ObservableList<Track>(new List<Track>(){track});
                _dataModel.QueueIndex = _dataModel.TrackQueue.IndexOf(track);
            }
        }
        private void _addTracksToQueue(object argument)
        {
            if (SelectedTracks == null) return;
            if (SelectedTracks.Count > 0)
            {
                CurrentPlaylist = null;
                foreach(Track track in SelectedTracks)
                {
                    if(!_dataModel.TrackQueue.Contains(track))
                        _dataModel.TrackQueue.Add(track);
                }
                RaisePropertyChanged("TrackQueue");
            }
        }
        #region playlist creation
        /// <summary>
        /// Open the dialog for adding a playlist.
        /// </summary>
        /// <param name="argument"></param>
        private async void _newPlaylistDialog(object argument)
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
                    Playlist playlist = _dataModel.AddPlaylist(dataContext.TextBoxText);
                    if(playlist != null)
                    {
                        _dataModel.AddTracksToPlaylist(SelectedTracks.ToList(), playlist);
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
            _filteredTracks = TrackList;
            _searchService.Itemlist = TrackList.ToArray();
            queueSearchTask();
        }
        private void sortListView()
        {
            Logger.Log("Sorting " + _sortedColumn);

            IEnumerable<Track> sortedList = null;

            if (_sortedColumn == HeaderType.Album)
            {
                if(_sortingType == SortingType.Ascending)
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
            if (_sortedColumn == HeaderType.Duration)
            {
                if (_sortingType == SortingType.Ascending)
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
            if (_sortedColumn == HeaderType.Genre)
            {
                if (_sortingType == SortingType.Ascending)
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
            if (_sortedColumn == HeaderType.Interpret)
            {
                if (_sortingType == SortingType.Ascending)
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
            if (_sortedColumn == HeaderType.Rating)
            {
                if (_sortingType == SortingType.Ascending)
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
            if (_sortedColumn == HeaderType.Title)
            {
                if (_sortingType == SortingType.Ascending)
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
            if (_sortedColumn == HeaderType.Year)
            {
                if (_sortingType == SortingType.Ascending)
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
                _filteredTracks = new ObservableCollection<Track>(sortedList);
        }
        #region searching methods
        /// <summary>
        /// Deletes the search text and updates the list.
        /// </summary>
        /// <param name="argument"></param>
        private void _deleteSearch(object argument)
        {
            if (_searchText.Equals(""))
                return;

            SearchBoxText = "";          
            RaisePropertyChanged("SearchBoxText");
            _searchText = "";
            queueSearchTask();  
        }
        private void OnSearchCompleted(object result)
        {
            _filteredTracks = new ObservableCollection<Track>((List<Track>)result);
            sortListView();
            RaisePropertyChanged("FilteredTracks");
            RaisePropertyChanged("TrackDurationSum");
        }
        /// <summary>
        /// Queues a search task and sorts and updates the view on completion.
        /// </summary>
        private void queueSearchTask()
        {
            _searchService.QueueSearchTask(_searchText);
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
                if (_sortingType == SortingType.Ascending)
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
                if (_sortedColumn == HeaderType.Title)
                    return true;

                return false;
            }
        }
        public bool InterpretSorted
        {
            get
            {
                if (_sortedColumn == HeaderType.Interpret)
                    return true;

                return false;
            }
        }
        public bool AlbumSorted
        {
            get
            {
                if (_sortedColumn == HeaderType.Album)
                    return true;

                return false;
            }
        }
        public bool YearSorted
        {
            get
            {
                if (_sortedColumn == HeaderType.Year)
                    return true;

                return false;
            }
        }
        public bool GenreSorted
        {
            get
            {
                if (_sortedColumn == HeaderType.Genre)
                    return true;

                return false;
            }
        }
        public bool RatingSorted
        {
            get
            {
                if (_sortedColumn == HeaderType.Rating)
                    return true;

                return false;
            }
        }
        public bool DurationSorted
        {
            get
            {
                if (_sortedColumn == HeaderType.Duration)
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
            foreach(Track track in _filteredTracks)
            {
                returnValue = TrackService.AddDurations(returnValue, track.Duration);
            }
            return returnValue;
        }
    }
}
