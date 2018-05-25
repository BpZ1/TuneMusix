using System.Windows.Controls;
using TuneMusix.Model;
using TuneMusix.ViewModel;
using System.Windows;
using System.Windows.Input;

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

        void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (TracklistViewModel)DataContext;
            var item = ((FrameworkElement)e.OriginalSource).DataContext as Track;
            if (item != null)
            {
                viewModel.TrackDoubleClicked.Execute(item);
            }
        }
    }
}
