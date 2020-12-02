using System;
using TuneMusix.Helpers;

namespace TuneMusix.Model
{
    public class Track : NotifyPropertyChangedBase, IDisposable
    {
        public Interpret InterpretContainer { get; set; }
        public Album AlbumContainer { get; set; }
        public bool IsModified { get; set; }
        public Folder Container { get; set; }
        public ObservableValue<string> SourceURL { get; set; }
        public ObservableValue<string> FolderID { get; set; }
        public ObservableValue<string> Album { get; set; }
        public ObservableValue<int> Year { get; set; }
        public ObservableValue<string> Genre { get; set; }
        public ObservableValue<int> Rating { get; set; }
        public ObservableValue<bool> IsValid { get; set; } = new ObservableValue<bool>();
        public ObservableValue<string> Comm { get; set; }
        public ObservableValue<bool> IsCurrentTrack { get; set; } = new ObservableValue<bool>();
        public ObservableValue<string> Interpret { get; set; }
        public ObservableValue<string> Title { get; set; }
        public string Id { get; private set; }
        public string Duration { get; private set; }
        public int Index { get; set; }

        public Track( string id, int year, string url, string title, string interpret,
            string album, string comm, string genre, string duration )
        {
            FolderID = new ObservableValue<string>( OnTrackChanged );
            Album = new ObservableValue<string>( album ?? string.Empty, OnTrackChanged );
            SourceURL = new ObservableValue<string>( string.Empty, OnTrackChanged );
            Year = new ObservableValue<int>( year, OnTrackChanged );
            Genre = new ObservableValue<string>( genre ?? string.Empty, OnTrackChanged );
            Rating = new ObservableValue<int>( OnTrackChanged );
            Comm = new ObservableValue<string>( comm ?? string.Empty, OnTrackChanged );
            Interpret = new ObservableValue<string>( interpret ?? string.Empty, () =>
            {
                NotifyPropertyChanged( "Interpret" );
                NotifyPropertyChanged( "Name" );
                OnTrackChanged();
            } );
            Title = new ObservableValue<string>( title ?? string.Empty, () =>
            {
                NotifyPropertyChanged( "Title" );
                NotifyPropertyChanged( "Name" );
                OnTrackChanged();
            } );
            SourceURL = new ObservableValue<string>( url );
            Id = id;
            Duration = duration;
        }

        public Track( string url, string id, string folderId, string title,
            string interpret, string album, int year, string comm, string genre, int rating, string duration ) : this( id, year, url, title, interpret, album, comm, genre, duration )
        {
            FolderID = new ObservableValue<string>( folderId );
            Rating.Value = rating;
        }

        public Track( int year, string url, string title, string interpret,
            string album, string comm, string genre, string duration ) : this( GenerateId(), year, url, title, interpret, album, comm, genre, duration ) { }

        //events
        public delegate void TrackChangedEventHandler( object source );

        public event TrackChangedEventHandler TrackChanged;

        protected virtual void OnTrackChanged()
        {
            IsModified = true;
            TrackChanged?.Invoke( this );
        }


        /// <summary>
        /// Removes the reference to the track from the folder it is in.
        /// </summary>
        public void Dispose()
        {
            if ( Container != null )
            {
                Container.Remove( this );
                AlbumContainer.Remove( this );
                InterpretContainer.Remove( this );
            }
        }

        /// <summary>
        /// Combination of Title and Interpret of the track.
        /// Form: "Title - Interpret"
        /// If one of them is empty "..." will be 
        /// </summary>
        public string Name
        {
            get
            {
                if ( string.IsNullOrEmpty( Interpret.Value ) )
                {
                    return Title.Value;
                }

                return $"{Title.Value} - {Interpret.Value}";
            }
        }

        public bool Contains( string value )
        {
            if ( Title.Value.ToLower().Contains( value.ToLower() ) )
            {
                return true;
            }

            if ( Interpret.Value.ToLower().Contains( value.ToLower() ) )
            {
                return true;
            }

            if ( Album.Value.ToLower().Contains( value.ToLower() ) )
            {
                return true;
            }

            return false;
        }

        public override bool Equals( object obj )
        {
            if ( !( obj is Track otherTrack ) )
            {
                return false;
            }
            return Id.Equals( otherTrack.Id );
        }

        private static string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
