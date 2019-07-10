
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
        private static readonly int coverArtResolution = 500;
        public static BitmapSource DefaultCover { get; set; }
        public Album(string name) : base(name) { }

        private BitmapSource _image;
        public string Duration { get; set; }
        public string Interpret { get; set; }

        public int TrackCount
        {
            get { return _itemlist.Count; }
        }

        protected override void OnContainerChanged()
        {
            UpdateDuration();
            UpdateInterpret();
            base.OnContainerChanged();
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

        public BitmapSource CoverArt
        {
            get
            {
                if(_image == null)
                {
                    LoadCoverArt();
                }
                return _image;
            }
            set { _image = value; }
        }

        /// <summary>
        /// Loads the cover art for the first found cover art.
        /// If no art is found on any of the tracks the default will be used.
        /// </summary>
        /// <returns></returns>
        public void LoadCoverArt()
        {
            if (Itemlist.Count > 0)
            {
                //Loop through all tracks of the album until an image is found
                foreach(Track track in Itemlist)
                {
                    TagLib.File file = TagLib.File.Create(track.SourceURL);
                    if(file.Tag.Pictures != null && file.Tag.Pictures.Length > 0)
                    {
                        var bin = (byte[])(file.Tag.Pictures[0].Data.Data);
                        //Load the first image of the file
                        using (var memoryStream = new MemoryStream(bin))
                        {
                            try
                            {
                                Image image = Image.FromStream(memoryStream).GetThumbnailImage
                              (
                              coverArtResolution,
                              coverArtResolution,
                              null,
                              IntPtr.Zero
                              );
                                CoverArt = Converter.ConvertBitmap(new Bitmap(image));
                            }catch(Exception e)
                            {
                                Logger.Log("Could not load cover art of track '" + track.Title + "'");
                                Logger.LogException(e);
                            }
                          
                        }
                        //If an image was found end loop
                        if (_image != null)
                        {
                            break;
                        }
                    }
                }
                //If no image was found load the default image.
                if (_image == null)
                {
                    SetDefaultCover();
                }
            }
        }

        private void SetDefaultCover()
        {
            if (DefaultCover == null)
                LoadDefaultAlbumCover();
                
            this.CoverArt = DefaultCover;
        }

        private void LoadDefaultAlbumCover()
        {
            BitmapSource image = Converter.ConvertBitmap(Resources.Resource.defaultAlbumCover);
            DefaultCover = image;
        }
    }
}
