using System.ComponentModel;
using TuneMusix.Data.SQLDatabase;
using TuneMusix.Helpers;
using TuneMusix.Helpers.Dialogs;
using TuneMusix.Helpers.MediaPlayer;
using TuneMusix.Model;
using TuneMusix.View.OptionsWindow;
using TuneMusix.ViewModel.Dialog;

namespace TuneMusix.ViewModel
{
    class OptionsOverviewViewModel
    {
        private Options _options = Options.Instance;

        public RelayCommand ExitOptionsWindow { get; set; }
        public RelayCommand Apply { get; set; }
        public RelayCommand Cancel { get; set; }

        public OptionsOverviewViewModel()
        {
            ExitOptionsWindow = new RelayCommand(_exitOptionsWindow);
            Apply = new RelayCommand(_apply);
            Cancel = new RelayCommand(_cancel);
        }

        /// <summary>
        /// Saves the options
        /// </summary>
        /// <param name="argument"></param>
        private void _apply(object argument)
        {           
            if (_options.IsModified())
            {
                _options.Save();
            }
        }

        private void _cancel(object argument)
        {
            OptionsWindowView optionsWindow = argument as OptionsWindowView;
            optionsWindow.Close();
        }
        /// <summary>
        /// Gets called when the window is closed.
        /// </summary>
        /// <param name="argument"></param>
        private void _exitOptionsWindow(object argument)
        {
            var eventArgs = argument as CancelEventArgs;

            if (_options.IsModified())
            {
                //Open a confirmation dialog to check if the user wants to save his changes.
                DialogResult result = OpenDialog("Do you want to save the changes?");

                if (result == DialogResult.Yes)
                {
                    _options.Save();
                    eventArgs.Cancel = false;
                }
                else if (result == DialogResult.No)
                {
                    //Restoring previous settings
                    SQLLoader loader = new SQLLoader();
                    loader.LoadOptions();
                    eventArgs.Cancel = false;
                }
                else if (result == DialogResult.Undefined)
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
        private DialogResult OpenDialog(string message)
        {
            DialogViewModelBase vm = new ConfirmationDialogViewModel(message);
            DialogResult result = DialogService.OpenDialog(vm);
            return result;
        }



    }
}
