using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneMusix.Model
{
    public class Track : BaseModel
    {
        private string URL;
        public string tTitle { get; set; }
        public string tInterpret { get; set; }
        public string tAlbum { get; set; }
        public string tYear { get; set; }
        public string tComm { get; set; }
        public string tGenre { get; set; }
        public int rating { get; set; }
        

        /// <summary>
        /// Constructor for Track
        /// </summary>
        /// <param name="URL"></param> 
        public Track(string URL)
        {
            this.URL = URL;
            rating = 0;
        }

        public string url
        {
            get
            {
                if (URL != null)
                {
                    return URL;
                }
                throw new NullReferenceException("URL");
            }
            set
            {
                if (value != null)
                {
                    URL = value;
                    RaisePropertyChanged("URL");
                }
                throw new ArgumentNullException("URL can't be null");
            }
        }


       
    }
}
