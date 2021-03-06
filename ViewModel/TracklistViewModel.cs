﻿using MaterialDesignThemes.Wpf;
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
        public ObservableList<Track> SelectedTracks { get; set; }
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

        private ObservableList<Track> _filteredTracks;

        //Constructor
        public TracklistViewModel()
        {
            _searchText = "";
            _sortingType = SortingType.Ascending;
            _sortedColumn = HeaderType.Title;
            _searchService = new DelayIterativeSearch( TrackList.ToArray() );
            _searchService.SearchTaskCompleted += OnSearchCompleted;
            SelectedTracks = new ObservableList<Track>();
            _filteredTracks = TrackList;
            queueSearchTask();

            #region commands
            DeleteSelectedTracks = new RelayCommand( _deleteSelectedTracks );
            AddToPlaylist = new RelayCommand( _addToPlaylist );
            SelectionChanged = new RelayCommand( _selectionChanged );
            PlayTrack = new RelayCommand( _playTrack );
            SearchChanged = new RelayCommand( _searchChanged );
            DeleteSearch = new RelayCommand( _deleteSearch );
            ColumnClicked = new RelayCommand( _columnClicked );
            TrackDoubleClicked = new RelayCommand( _trackDoubleClicked );
            AddTracksToQueue = new RelayCommand( _addTracksToQueue );
            OpenNewPlaylistDialog = new RelayCommand( _newPlaylistDialog );
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
            get { return $"Selected items: {SelectedTracks.Count}"; }
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
        private void _deleteSelectedTracks( object argument )
        {
            _dataModel.Delete( SelectedTracks.ToList<Track>() );
        }
        /// <summary>
        /// Adds all selected tracks to the playlist with the given id.
        /// </summary>
        /// <param name="argument"></param>
        private void _addToPlaylist( object argument )
        {
            Playlist selectedPlaylist = argument as Playlist;

            if ( selectedPlaylist != null )
            {
                _dataModel.AddTracksToPlaylist( SelectedTracks.ToList<Track>(), selectedPlaylist );
            }
        }
        private void _selectionChanged( object argument )
        {
            var listView = argument as ListView;
            if ( listView == null ) return;

            SelectedTracks.Clear();
            //Create a temporary list to allow mass insertion for performance savings.
            List<Track> currentSelection = new List<Track>();
            foreach ( Track track in listView.SelectedItems )
            {
                currentSelection.Add( track );
            }
            SelectedTracks.AddRange( currentSelection );
            RaisePropertyChanged( "SelectedItemsText" );
        }
        private void _playTrack( object argument )
        {
            if ( SelectedTracks == null ) return;
            if ( SelectedTracks.Count > 0 )
            {
                CurrentPlaylist = null;
                TrackQueue = new ObservableList<Track>( SelectedTracks );
            }
        }
        /// <summary>
        /// Called every time the text in the textbox changes
        /// </summary>
        /// <param name="argument"></param>
        private void _searchChanged( object argument )
        {
            if ( ( ( string ) argument ).Equals( _searchText ) )
                return;

            _searchText = ( string ) argument;
            queueSearchTask();
        }
        /// <summary>
        /// Changes the type of the column to sort.
        /// Changes the sorting type if the same column is
        /// clicked that is already sorted.
        /// </summary>
        /// <param name="argument"></param>
        private void _columnClicked( object argument )
        {
            var columnType = argument as string;
            HeaderType previousType = _sortedColumn;
            switch ( columnType )
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
            if ( previousType == _sortedColumn )
            {
                if ( _sortingType == SortingType.Ascending )
                {
                    _sortingType = SortingType.Descending;
                }
                else
                {
                    _sortingType = SortingType.Ascending;
                }
                RaisePropertyChanged( "SortingIcon" );
            }
            RaisePropertyChanged( "TitleSorted" );
            RaisePropertyChanged( "InterpretSorted" );
            RaisePropertyChanged( "AlbumSorted" );
            RaisePropertyChanged( "YearSorted" );
            RaisePropertyChanged( "GenreSorted" );
            RaisePropertyChanged( "RatingSorted" );
            RaisePropertyChanged( "DurationSorted" );
            //start sorting
            SortListView();
            RaisePropertyChanged( "FilteredTracks" );
        }
        private void _trackDoubleClicked( object argument )
        {
            if ( argument == null )
                throw new ArgumentNullException();

            var track = argument as Track;
            if ( track != null )
            {
                CurrentTrack = track;
                _dataModel.CurrentPlaylist = null;
            }
        }
        private void _addTracksToQueue( object argument )
        {
            if ( SelectedTracks == null ) return;
            if ( SelectedTracks.Count > 0 )
            {
                CurrentPlaylist = null;
                foreach ( Track track in SelectedTracks )
                {
                    _dataModel.TrackQueue.Add( track );
                }
                RaisePropertyChanged( "TrackQueue" );
            }
        }
        #region playlist creation
        /// <summary>
        /// Open the dialog for adding a playlist.
        /// </summary>
        /// <param name="argument"></param>
        private async void _newPlaylistDialog( object argument )
        {
            var view = new GetTextDialog
            {
                DataContext = new GetTextViewModel( "New playlist" )
            };
            //show the dialog
            var result = await DialogHost.Show( view, "DialogHost", OpenedEventHandler, playlistDialogClosingHandler );
        }
        /// <summary>
        /// Intercept the open and affect the dialog using eventArgs.Session.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventargs"></param>
        private void OpenedEventHandler( object sender, DialogOpenedEventArgs eventargs )
        {
        }

        /// <summary>
        /// Closes the dialog for adding a playlist and adds the playlist if accept was selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void playlistDialogClosingHandler( object sender, DialogClosingEventArgs eventArgs )
        {
            if ( ( bool ) eventArgs.Parameter == false ) return;

            //chancel the closing
            eventArgs.Cancel();
            var content = eventArgs.Session.Content as GetTextDialog;
            eventArgs.Session.UpdateContent( new GetTextDialog() );
            var dataContext = content.DataContext as GetTextViewModel;
            if ( dataContext != null )
            {
                if ( dataContext.TextBoxText.Count<char>() >= 2 )
                {
                    Playlist playlist = _dataModel.AddPlaylist( dataContext.TextBoxText );
                    if ( playlist != null )
                    {
                        _dataModel.AddTracksToPlaylist( SelectedTracks.ToList(), playlist );
                    }
                }
                else
                {
                    DialogService.NotificationMessage( "The name has to be longer than two characters." );
                }
            }
            Task.Factory.StartNew( () => { } ).ContinueWith( ( t, _ ) => eventArgs.Session.Close( false ), null,
                         TaskScheduler.FromCurrentSynchronizationContext() );

        }
        #endregion
        #endregion

        #region methods
        private void OnTrackListChanged( object source, object obj )
        {
            _filteredTracks = TrackList;
            _searchService.Itemlist = TrackList.ToArray();
            queueSearchTask();
        }
        private void SortListView()
        {
            Logger.Log( $"Sorting {_sortedColumn}" );

            IEnumerable<Track> sortedList = null;

            if ( _sortedColumn == HeaderType.Album )
            {
                if ( _sortingType == SortingType.Ascending )
                {
                    sortedList = FilteredTracks.OrderBy( track => track.Album.Value );
                }
                else
                {
                    sortedList = FilteredTracks.OrderByDescending( track => track.Album.Value );
                }
            }
            if ( _sortedColumn == HeaderType.Duration )
            {
                if ( _sortingType == SortingType.Ascending )
                {
                    sortedList = FilteredTracks.OrderBy( track => track.Duration );
                }
                else
                {
                    sortedList = FilteredTracks.OrderByDescending( track => track.Duration );
                }
            }
            if ( _sortedColumn == HeaderType.Genre )
            {
                if ( _sortingType == SortingType.Ascending )
                {
                    sortedList = FilteredTracks.OrderBy( track => track.Genre.Value );
                }
                else
                {
                    sortedList = FilteredTracks.OrderByDescending( track => track.Genre.Value );
                }
            }
            if ( _sortedColumn == HeaderType.Interpret )
            {
                if ( _sortingType == SortingType.Ascending )
                {
                    sortedList = FilteredTracks.OrderBy( track => track.Interpret.Value );
                }
                else
                {
                    sortedList = FilteredTracks.OrderByDescending( track => track.Interpret.Value );
                }
            }
            if ( _sortedColumn == HeaderType.Rating )
            {
                if ( _sortingType == SortingType.Ascending )
                {
                    sortedList = FilteredTracks.OrderBy( track => track.Rating.Value );
                }
                else
                {
                    sortedList = FilteredTracks.OrderByDescending( track => track.Rating.Value );
                }
            }
            if ( _sortedColumn == HeaderType.Title )
            {
                if ( _sortingType == SortingType.Ascending )
                {
                    sortedList = FilteredTracks.OrderBy( track => track.Title.Value );
                }
                else
                {
                    sortedList = FilteredTracks.OrderByDescending( track => track.Title.Value );
                }
            }
            if ( _sortedColumn == HeaderType.Year )
            {
                if ( _sortingType == SortingType.Ascending )
                {
                    sortedList = FilteredTracks.OrderBy( track => track.Year.Value );
                }
                else
                {
                    sortedList = FilteredTracks.OrderByDescending( track => track.Year.Value );
                }
            }
            if ( sortedList != null )
            {
                _filteredTracks = new ObservableList<Track>( sortedList );
            }
        }
        #region searching methods
        /// <summary>
        /// Deletes the search text and updates the list.
        /// </summary>
        /// <param name="argument"></param>
        private void _deleteSearch( object argument )
        {
            if ( _searchText.Equals( "" ) )
                return;

            SearchBoxText = "";
            RaisePropertyChanged( "SearchBoxText" );
            _searchText = "";
            queueSearchTask();
        }
        private void OnSearchCompleted( object sender, object result )
        {
            _filteredTracks = new ObservableList<Track>( ( List<Track> ) result );
            SortListView();
            RaisePropertyChanged( "FilteredTracks" );
            RaisePropertyChanged( "TrackDurationSum" );
        }
        /// <summary>
        /// Queues a search task and sorts and updates the view on completion.
        /// </summary>
        private void queueSearchTask()
        {
            _searchService.QueueSearchTask( _searchText );
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
                if ( _sortingType == SortingType.Ascending )
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
                return _sortedColumn == HeaderType.Title;
            }
        }
        public bool InterpretSorted
        {
            get
            {
                return _sortedColumn == HeaderType.Interpret;
            }
        }
        public bool AlbumSorted
        {
            get
            {
                return _sortedColumn == HeaderType.Album;
            }
        }
        public bool YearSorted
        {
            get
            {
                return _sortedColumn == HeaderType.Year;
            }
        }
        public bool GenreSorted
        {
            get
            {
                return _sortedColumn == HeaderType.Genre;
            }
        }
        public bool RatingSorted
        {
            get
            {
                return _sortedColumn == HeaderType.Rating;
            }
        }
        public bool DurationSorted
        {
            get
            {
                return _sortedColumn == HeaderType.Duration;
            }
        }

        public string TrackDurationSum
        {
            get
            {
                return $"Total playtime: [{sumAllTracks()}]";
            }
        }
        private string sumAllTracks()
        {
            string returnValue = "";
            foreach ( Track track in _filteredTracks )
            {
                returnValue = TrackService.AddDurations( returnValue, track.Duration );
            }
            return returnValue;
        }
    }
}
