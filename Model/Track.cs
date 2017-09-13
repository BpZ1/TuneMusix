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
        private string _title;
        private string _interpret;
        private string _album;
        private string _year;
        private string _comm;
        private string _genre;
        private int _rating;
        

        /// <summary>
        /// Constructor for Track
        /// </summary>
        /// <param name="URL"></param> 
        public Track(string URL)
        {
            this.URL = URL;
        }

        //getter and setter
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

        public string Title
        {
            get
            {
                if(this._title != null)
                {
                    return this._title;
                }
                return "";
            }
            set
            {
                this._title = value;
            }
        }
        public string Interpret
        {
            get
            {
                if (this._interpret != null)
                {
                    return this._interpret;
                }
                return "";
            }
            set
            {
                this._interpret = value;
            }
        }
        public string Album
        {
            get
            {
                if (this._album != null)
                {
                    return this._album;
                }
                return "";
            }
            set
            {
                this._album = value;
            }
        }
        public string Year
        {
            get
            {
                if (this._year != null)
                {
                    return this._year;
                }
                return "";
            }
            set
            {
                this._year = value;
            }
        }
        public string Comm
        {
            get
            {
                if (this._comm != null)
                {
                    return this._comm;
                }
                return "";
            }
            set
            {
                this._comm = value;
            }
        }
        public string Genre
        {
            get
            {
                if (this._genre != null)
                {
                    return this._genre;
                }
                return "";
            }
            set
            {
                this._genre = value;
            }
        }
        public int Rating
        {
            get
            {
                return this._rating;
            }
            set
            {
                this._rating = value;
            }
        }
        public string Name
        {
            get
            {
                if(this.Interpret == null)
                {
                    return this.Title;
                }
                return this.Title + " - " + this.Interpret;
            }
        }
    }
}
