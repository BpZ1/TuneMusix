using System;
using System.Windows.Threading;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer;

namespace TuneMusix.ViewModel
{
    class VisualizerViewModel : ViewModelBase
    {
        private float[] _fftData = new float[1023];
        private float[] _barValues = new float[DEFAULT_BAR_COUNT];
        private int _barCount = DEFAULT_BAR_COUNT;
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        private const int DEFAULT_BAR_COUNT = 20;
        private const int MAX_DB_VALUE = 0;
        private const int MIN_DB_VALUE = -90;
        private const int DB_SCALE = (MAX_DB_VALUE - MIN_DB_VALUE);

        public VisualizerViewModel()
        {
            AudioControls.Instance.Paused += OnPlayerPaused;
            AudioControls.Instance.Stopped += OnPlayerPaused;
            AudioControls.Instance.Playing += OnPlayerPlays;

            IsActive = true;
            RaisePropertyChanged("IsActive");
            _timer.Interval = TimeSpan.FromMilliseconds(60);
            _timer.Tick += OnTick;
            _timer.Start();
        }

        private void OnPlayerPaused(object o)
        {
            //Set the bar values to zero if no music is played.
            for(int i = 0; i < _barValues.Length; i++)
            {
                _barValues[i] = 0f;
            }
            _timer.Stop();
        }

        private void OnPlayerPlays(object o)
        {
            _timer.Start();
        }

        public bool IsActive { get; set; }

        public int BarCount
        {
            get { return _barCount; }
            set
            {
                _barCount = value;
                RaisePropertyChanged("BarCount");
                BarValues = new float[value];
            }
        }

        public float[] BarValues
        {
            get { return _barValues; }
            set
            {
                _barValues = value;
                RaisePropertyChanged("BarValues");
            }
        }

        private void OnTick(object sender, EventArgs args)
        {
            UpdateVisualData();
        }

        private void UpdateVisualData()
        {
            //Get updated fft data
            AudioControls.Instance.GetFFTData(_fftData);

            int dataPerBar = _fftData.Length / BarCount;

            double currentMaximumHeight = 0;

            int barCounter = 0;
            int counter = 0;
            for (int i = 0; i < _fftData.Length; i++)
            {
                double dbValue = 20 * Math.Log10(_fftData[i]);
                double height = (dbValue - MIN_DB_VALUE) / DB_SCALE * 100;
                //The maximum of the bins will be used.
                currentMaximumHeight = Math.Max(currentMaximumHeight, height);
                counter++;
                //If the last bin for the current bar has been reached
                if(counter == dataPerBar)
                {
                    counter = 0;
                    _barValues[barCounter] = (float) currentMaximumHeight;
                    currentMaximumHeight = 0;
                    barCounter++;
                }
            }
        }

    }
}
