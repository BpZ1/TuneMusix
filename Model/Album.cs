using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using TuneMusix.Helpers;

namespace TuneMusix.Model
{
    public class Album : ItemContainer<Track>
    {
        private static readonly int _coverArtResolution = 200;
        private static BitmapSource _defaultCover;
        private BitmapSource _image;
        public ObservableValue<string> Interpret = new ObservableValue<string>( string.Empty );
        public ObservableValue<string> Duration = new ObservableValue<string>( string.Empty );
        public ObservableValue<string> Genre = new ObservableValue<string>( string.Empty );
        public ObservableValue<int> Year = new ObservableValue<int>();

        public Album( string name ) : base( name ) { }
        #region Properties
        public static BitmapSource DefaultCover
        {
            get
            {
                if ( _defaultCover == null )
                {
                    _defaultCover = Converter.ConvertBitmap( Resources.Resource.defaultAlbumCover );
                }

                return _defaultCover;
            }
        }

        public int TrackCount
        {
            get { return _itemlist.Count; }
        }

        public BitmapSource CoverArt
        {
            get
            {
                if ( _image == null )
                {
                    _image = LoadImage();
                }
                return _image;
            }
            set
            {
                _image = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        protected override void OnContainerChanged()
        {
            UpdateDuration();
            UpdateInterpret();
            UpdateGenre();
            if ( Itemlist.Count > 0 )
            {
                Track track = Itemlist[0];
                Year.Value = track.Year.Value;
            }
            base.OnContainerChanged();
        }

        private void UpdateGenre()
        {
            string genre = string.Empty;
            //Check which non duplicate genres are found
            HashSet<string> uniqueNames = new HashSet<string>();
            foreach ( Track track in Itemlist )
            {
                if ( uniqueNames.Add( track.Interpret.Value.ToLower() ) )
                {
                    if ( !string.IsNullOrEmpty( genre ) )
                    {
                        genre += ", " + track.Genre;
                    }
                    else
                    {
                        genre += track.Genre;
                    }
                }
            }
            //Set the interpret to unknown if none was found.
            if ( genre.Equals( "" ) )
            {
                genre = DefaultName;
            }

            Genre.Value = genre;
        }

        private void UpdateDuration()
        {
            string duration = "00:00:00";
            foreach ( Track track in Itemlist )
            {
                duration = TrackService.AddDurations( duration, track.Duration );
            }
            this.Duration.Value = duration;
        }

        private void UpdateInterpret()
        {
            string interpret = "";
            //Check which non duplicate interprets are found
            HashSet<string> uniqueNames = new HashSet<string>();
            foreach ( Track track in Itemlist )
            {
                if ( uniqueNames.Add( track.Interpret.Value.ToLower() ) )
                {
                    if ( !interpret.Equals( "" ) )
                    {
                        interpret += ", " + track.Interpret;
                    }
                    else
                    {
                        interpret += track.Interpret;
                    }
                }
            }
            //Set the interpret to unknown if none was found.
            if ( string.IsNullOrEmpty( interpret ) )
            {
                interpret = DefaultName;
            }
            Interpret.Value = interpret;
        }

        private BitmapSource LoadImage()
        {
            BitmapSource result = null;
            if ( Itemlist.Count > 0 )
            {
                //Loop through all tracks of the album until an image is found
                foreach ( Track track in Itemlist )
                {
                    TagLib.File file = TagLib.File.Create( track.SourceURL.Value );
                    if ( file.Tag.Pictures != null && file.Tag.Pictures.Length > 0 )
                    {
                        var bin = file.Tag.Pictures[0].Data.Data;
                        //Load the first image of the file
                        using ( var memoryStream = new MemoryStream( bin ) )
                        {
                            try
                            {
                                Image image = Image.FromStream( memoryStream ).GetThumbnailImage
                              (
                              _coverArtResolution,
                              _coverArtResolution,
                              null,
                              IntPtr.Zero
                              );
                                result = Converter.ConvertBitmap( new Bitmap( image ) );
                            }
                            catch ( Exception e )
                            {
                                Logger.Log( $"Could not load cover art of track '{track.Title}'" );
                                Logger.LogException( e );
                            }
                        }
                        //If an image was found end loop
                        if ( result != null )
                        {
                            break;
                        }
                    }
                }
                //If no image was found load the default image.
                if ( result == null )
                {
                    result = Album.DefaultCover;
                }
            }
            return result;
        }

        public override bool Equals( object obj )
        {
            if ( !( obj is Album otherAlbum ) )
            {
                return false;
            }
            return base.Equals( obj );
        }

        public override int GetHashCode()
        {
            var hashCode = 1978926223;
            hashCode = hashCode * -1521134295 + Interpret.Value.GetHashCode();
            hashCode = hashCode * -1521134295 + Duration.Value.GetHashCode();
            hashCode = hashCode * -1521134295 + Genre.Value.GetHashCode();
            hashCode = hashCode * -1521134295 + Year.Value.GetHashCode();
            return hashCode + base.GetHashCode();
        }

        private const string DefaultName = "Unknown";
    }
}
