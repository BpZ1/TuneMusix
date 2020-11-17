using System.ComponentModel;
using TuneMusix.Data.SQLDatabase;
using TuneMusix.Helpers;
using TuneMusix.Helpers.Dialogs;
using TuneMusix.Model;
using TuneMusix.View.OptionsWindow;
using TuneMusix.ViewModel.Dialog;

namespace TuneMusix.ViewModel.OptionsViewModel
{
    class OptionsOverviewViewModel
    {
        private Options _options = Options.Instance;

        public RelayCommand ExitOptionsWindowCommand { get; set; }
        public RelayCommand ApplyCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public OptionsOverviewViewModel()
        {
            ExitOptionsWindowCommand = new RelayCommand( ExitOptionsWindow );
            ApplyCommand = new RelayCommand( Apply );
            CancelCommand = new RelayCommand( Cancel );
        }

        /// <summary>
        /// Saves the options
        /// </summary>
        /// <param name="argument"></param>
        private void Apply( object argument )
        {           
            if ( _options.IsModified() )
            {
                _options.Save();
            }
        }

        private void Cancel( object argument )
        {
            OptionsWindowView optionsWindow = argument as OptionsWindowView;
            optionsWindow.Close();
        }
        /// <summary>
        /// Gets called when the window is closed.
        /// </summary>
        /// <param name="argument"></param>
        private void ExitOptionsWindow(object argument)
        {
            var eventArgs = argument as CancelEventArgs;

            if ( _options.IsModified() )
            {
                //Open a confirmation dialog to check if the user wants to save his changes.
                DialogResult result = OpenDialog("Do you want to save the changes?");

                if (result == DialogResult.Yes)
                {
                    _options.Save();
                    eventArgs.Cancel = false;
                }
                else if ( result == DialogResult.No )
                {
                    //Restoring previous settings
                    SQLLoader loader = new SQLLoader();
                    loader.LoadOptions();
                    eventArgs.Cancel = false;
                }
                else if ( result == DialogResult.Undefined )
                {
                    eventArgs.Cancel = true;
                }
            }
            else
            {
                eventArgs.Cancel = false;
            }
        }
        /// <summary>
        /// Opens a window and asks the user for confirmation.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private DialogResult OpenDialog( string message )
        {
            DialogViewModelBase vm = new ConfirmationDialogViewModel(message);
            DialogResult result = DialogService.OpenDialog(vm);
            return result;
        }



    }
}
