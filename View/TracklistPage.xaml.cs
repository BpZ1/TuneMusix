using System.Windows.Controls;
using TuneMusix.Data;
using System;
using TuneMusix.Helpers;
using TuneMusix.Model;
using TuneMusix.ViewModel;

namespace TuneMusix.View
{
    /// <summary>
    /// Interaction logic for TracklistPage.xaml
    /// </summary>
    public partial class TracklistPage : Page
    {
        public TracklistPage()
        {
            InitializeComponent();
        }

        private void TrackList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            if (listView == null) return;

            var viewModel = listView.DataContext as TracklistViewModel;
            if (viewModel == null) return;
                   
            viewModel.SelectedTracks.Clear();

            foreach (Track track in listView.SelectedItems)
            {
                viewModel.SelectedTracks.Add(track);
            }
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
