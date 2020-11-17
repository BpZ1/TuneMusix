using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TuneMusix.Model;
using TuneMusix.ViewModel;

namespace TuneMusix.View
{
    /// <summary>
    /// Interaction logic for TrackQueuePage.xaml
    /// </summary>
    public partial class TrackQueuePage : Page
    {
        public TrackQueuePage()
        {
            InitializeComponent();
        }

        void ListView_MouseDoubleClick( object sender, MouseButtonEventArgs e )
        {
            var viewModel = ( TrackQueueViewModel ) DataContext;
            var item = ( ( FrameworkElement ) e.OriginalSource ).DataContext as Track;
            if ( item != null )
            {
                viewModel.TrackDoubleClickedCommand.Execute( item );
            }
        }
    }
}
