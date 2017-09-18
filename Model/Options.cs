using TuneMusix.Helpers;

namespace TuneMusix.Model
{
    public class Options
    {
        private static Options instance;

        private Options() { }

        public static Options Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Options();
                }
                return instance;
            }
        }

        private bool _LoggerActive = false;
        private int _Volume;
        public bool Shuffle { get; set; }
        public int RepeatTrack { get; set; }

        //Getter and setter
        public int Volume
        {
            get { return this._Volume; }
            set
            {
                if (value > 100)
                {
                    this._Volume = 100;
                }
                else
                {
                    this._Volume = value;
                }

            }
        }
        public bool LoggerActive
        {
            get { return this._LoggerActive; }
            set { this._LoggerActive = value; }
        }





    }
}
