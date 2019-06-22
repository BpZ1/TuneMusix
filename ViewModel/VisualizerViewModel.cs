using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer;

namespace TuneMusix.ViewModel
{
    class VisualizerViewModel : ViewModelBase
    {
        private float[] fftData = new float[1023];
        private const int DEFAULT_BAR_COUNT = 20;
        private float[] barValues = new float[DEFAULT_BAR_COUNT];
        private int barCount = DEFAULT_BAR_COUNT;
        private Random r = new Random();
        private DispatcherTimer timer = new DispatcherTimer();

        private const int maxDbValue = 0;
        private const int minDbValue = -90;
        private const int dbScale = (maxDbValue - minDbValue);

        public bool IsActive { get; set; }

        public int BarCount
        {
            get { return barCount; }
            set
            {
                barCount = value;
                RaisePropertyChanged("BarCount");
                BarValues = new float[value];
            }
        }

        public float[] BarValues
        {
            get { return barValues; }
            set
            {
                barValues = value;
                RaisePropertyChanged("BarValues");
            }
        }

        public RelayCommand Test { get; set; }

        public VisualizerViewModel()
        {
            IsActive = true;
            RaisePropertyChanged("IsActive");
            timer.Interval = TimeSpan.FromMilliseconds(60);
            timer.Tick += onTick;
            timer.Start();
        }

        private void onTick(object sender, EventArgs args)
        {
            updateVisualData();
        }

        private void updateVisualData()
        {
            //Get updated fft data
            AudioControls.Instance.GetFFTData(fftData);

            int dataPerBar = fftData.Length / BarCount;

            double currentMaximumHeight = 0;

            int barCounter = 0;
            int counter = 0;
            for (int i = 0; i < fftData.Length; i++)
            {
                double dbValue = 20 * Math.Log10(fftData[i]);
                double height = (dbValue - minDbValue) / dbScale * 100;
                //The maximum of the bins will be used.
                currentMaximumHeight = Math.Max(currentMaximumHeight, height);
                counter++;
                //If the last bin for the current bar has been reached
                if(counter == dataPerBar)
                {
                    counter = 0;
                    barValues[barCounter] = (float) currentMaximumHeight;
                    currentMaximumHeight = 0;
                    barCounter++;
                }
            }
        }

    }
}
