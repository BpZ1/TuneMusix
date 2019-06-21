using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer;
using WPFSoundVisualizationLib;

namespace TuneMusix.ViewModel
{
    class VisualizerViewModel : ViewModelBase
    {

        private float[] testData = new float[2048];
        private Random r = new Random();
        private bool changed = false;

        public bool IsActive { get; set; }

        public float[] TestData
        {
            get
            {
                return testData;
            }
        }

        private void changeData()
        {         
            for(int i = 0; i < testData.Length; i++)
            {
                testData[i] = r.Next(0,200);
            }
            changed = true;
            RaisePropertyChanged("TestData");
        }

        public RelayCommand Test { get; set; }

        public VisualizerViewModel()
        {
            Test = new RelayCommand(test);
            for(int i = 0; i < testData.Length; i++)
            {
                testData[i] = 50;
            }
        }

        private void test(object sender)
        {
            if (!IsActive)
            {
                IsActive = true;
                RaisePropertyChanged("IsActive");
            }
            else
            {
                for (int i = 0; i < testData.Length; i++)
                {
                    testData[i] += 10;
                }
            }        
        }
    }
}
