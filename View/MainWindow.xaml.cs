using System.Windows;
using System.Windows.Controls;
using System;
using TuneMusix.ViewModel;

namespace TuneMusix.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {          
            InitializeComponent();
        }



        /* not needed for now
        private void TrackList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            if (listView == null) return;

            var viewModel = listView.DataContext as ViewModelMain;
            if (viewModel == null) return;

            viewModel.SelectedTracks.Clear();

            foreach (Track track in listView.SelectedItems)
            {
                viewModel.SelectedTracks.Add(track);

            }
        }
        */
    }
}
