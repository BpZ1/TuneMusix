using System;
using System.Windows.Controls;
using TuneMusix.Data;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.View
{
    /// <summary>
    /// Interaction logic for FolderPage.xaml
    /// </summary>
    public partial class FolderPage : Page
    {
        public FolderPage()
        {
            InitializeComponent();
        }

        private void AllTrackList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Console.WriteLine(sender.GetType().ToString());
            var listView = sender as ListView;
            var clickedTrack = listView.SelectedItem as Track;
            if (clickedTrack == null)
            {
                return;
            }
            DataModel dataModel = DataModel.Instance;
            AudioControls audio = AudioControls.Instance;
            if (dataModel.CurrentTrack == clickedTrack)
            {
                return;
            }
            else
            {
                dataModel.CurrentTrack = clickedTrack;
                audio.PlayTrack();
            }
        }
    }
}
