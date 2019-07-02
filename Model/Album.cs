
using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using TuneMusix.Helpers;

namespace TuneMusix.Model
{
    public class Album : ItemContainer<Track>
    {
        private static readonly string _defaultAlbumCover = "Resources/defaultAlbumCover.png";
        public Album(string name) : base(name) { }
        
        private BitmapSource _image;
        public int TrackCount
        {
            get { return _itemlist.Count; }
        }
        public BitmapSource Image
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
                        BitmapImage bitmap;
                        using (var memoryStream = new MemoryStream(bin))
                        {
                            try
                            {
                                memoryStream.Seek(0, SeekOrigin.Begin);
                                
                                bitmap = new BitmapImage();
                                bitmap.BeginInit();
                                bitmap.StreamSource = memoryStream;
                                bitmap.EndInit();
                                _image = bitmap;
                            }
                            catch (NotSupportedException e)
                            {
                                Logger.Log("Could not read cover image of '" + file.Tag.Title + "'.");
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
                    LoadDefaultAlbumCover();
                }
            }
        }

        private void LoadDefaultAlbumCover()
        {
            FileStream fileStream = new FileStream(_defaultAlbumCover, FileMode.Open, FileAccess.Read);

            var img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = fileStream;
            img.EndInit();
            this._image = img;
            if (_image == null)
            {
                throw new FileNotFoundException("Could not find file at '" + _defaultAlbumCover + "'.");
            }
        }
    }
}
