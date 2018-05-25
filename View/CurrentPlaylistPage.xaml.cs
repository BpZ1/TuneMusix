using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TuneMusix.Model;
using TuneMusix.ViewModel;

namespace TuneMusix.View
{
    /// <summary>
    /// Interaction logic for CurrentPlaylistPage.xaml
    /// </summary>
    public partial class CurrentPlaylistPage : Page
    {
        public CurrentPlaylistPage()
        {
            InitializeComponent();          
        }

        void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (CurrentPlaylistViewModel)DataContext;
            var item = ((FrameworkElement)e.OriginalSource).DataContext as Track;
            if (item != null)
            {
                viewModel.TrackDoubleClicked.Execute(item);
            }
        }

    }
}
