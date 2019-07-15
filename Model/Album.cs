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
        private static readonly int _coverArtResolution = 500;
        private static BitmapSource _defaultCover;
        private BitmapSource _image;
        private string _interpret;
        private string _duration;
        private string _genre;
        private int _year;

        public Album(string name) : base(name) { }
        #region Properties
        public static BitmapSource DefaultCover
        {
            get
            {
                if(_defaultCover == null)
                    _defaultCover = Converter.ConvertBitmap(Resources.Resource.defaultAlbumCover);

                return _defaultCover;
            }
        }
        public string Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                RaisePropertyChanged("Duration");
            }
        }
        public string Interpret
        {
            get { return _interpret; }
            set
            {
                _interpret = value;
                RaisePropertyChanged("Interpret");
            }
        }
        public string Genre
        {
            get { return _genre; }
            set
            {
                _genre = value;
                RaisePropertyChanged("Genre");
            }
        }

        public int Year
        {
            get { return _year; }
            set
            {
                _year = value;
                RaisePropertyChanged("Year");
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
                if(_image == null)
                {
                    _image = LoadImage();
                }
                return _image;
            }
            set
            {
                _image = value;
                RaisePropertyChanged("CoverArt");
            }
        }
        #endregion

        protected override void OnContainerChanged()
        {
            UpdateDuration();
            UpdateInterpret();
            UpdateGenre();
            if (Itemlist.Count > 0)
            {
                Track track = Itemlist[0];
                Year = track.Year;
            }
            base.OnContainerChanged();
        }

        private void UpdateGenre()
        {
            string genre = "";
            //Check which non duplicate genres are found
            HashSet<string> uniqueNames = new HashSet<string>();
            foreach (Track track in Itemlist)
            {
                if (uniqueNames.Add(track.Interpret.ToLower()))
                {
                    if (!genre.Equals(""))
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
            if (genre.Equals(""))
            {
                genre = "Unknown";
            }

            Interpret = genre;
        }

        private void UpdateDuration()
        {
            string duration = "00:00:00";
            foreach (Track track in Itemlist)
            {
                duration = TrackService.AddDurations(duration, track.Duration);
            }
            this.Duration = duration;
        }

        private void UpdateInterpret()
        {
            string interpret = "";
            //Check which non duplicate interprets are found
            HashSet<string> uniqueNames = new HashSet<string>();
            foreach(Track track in Itemlist)
            {
                if (uniqueNames.Add(track.Interpret.ToLower()))
                {
                    if (!interpret.Equals(""))
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
            if (interpret.Equals(""))
            {
                interpret = "Unknown";
            }
            Interpret = interpret;
        }

        private BitmapSource LoadImage()
        {
            BitmapSource result = null;
            if (Itemlist.Count > 0)
            {
                //Loop through all tracks of the album until an image is found
                foreach (Track track in Itemlist)
                {
                    TagLib.File file = TagLib.File.Create(track.SourceURL);
                    if (file.Tag.Pictures != null && file.Tag.Pictures.Length > 0)
                    {
                        var bin = (byte[])(file.Tag.Pictures[0].Data.Data);
                        //Load the first image of the file
                        using (var memoryStream = new MemoryStream(bin))
                        {
                            try
                            {
                                Image image = Image.FromStream(memoryStream).GetThumbnailImage
                              (
                              _coverArtResolution,
                              _coverArtResolution,
                              null,
                              IntPtr.Zero
                              );
                                result = Converter.ConvertBitmap(new Bitmap(image));
                            }
                            catch (Exception e)
                            {
                                Logger.Log("Could not load cover art of track '" + track.Title + "'");
                                Logger.LogException(e);
                            }

                        }
                        //If an image was found end loop
                        if (result != null)
                        {
                            break;
                        }
                    }
                }
                //If no image was found load the default image.
                if (result == null)
                {
                    result = Album.DefaultCover;
                }
            }
            return result;
        }
    }
}
