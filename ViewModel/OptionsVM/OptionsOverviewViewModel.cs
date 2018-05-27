using System.ComponentModel;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Data.SQLDatabase;
using TuneMusix.Helpers;
using TuneMusix.Helpers.Dialogs;
using TuneMusix.Model;
using TuneMusix.View.OptionsWindow;
using TuneMusix.ViewModel.Dialog;

namespace TuneMusix.ViewModel
{
    class OptionsOverviewViewModel
    {
        private Options options = Options.Instance;
        private DataModel dataModel = DataModel.Instance;


        public RelayCommand ExitOptionsWindow { get; set; }
        public RelayCommand Apply { get; set; }
        public RelayCommand Cancel { get; set; }

        public OptionsOverviewViewModel()
        {
            ExitOptionsWindow = new RelayCommand(exitOptionsWindow);
            Apply = new RelayCommand(apply);
            Cancel = new RelayCommand(cancel);
        }
        /// <summary>
        /// Saves the options
        /// </summary>
        /// <param name="argument"></param>
        private void apply(object argument)
        {           
            if (options.IsModified())
            {
                options.Save();
            }
        }

        private void cancel(object argument)
        {
            OptionsWindowView optionsWindow = argument as OptionsWindowView;
            optionsWindow.Close();
        }
        /// <summary>
        /// Gets called when the window is closed.
        /// </summary>
        /// <param name="argument"></param>
        private void exitOptionsWindow(object argument)
        {
            var eventArgs = argument as CancelEventArgs;

            if (options.IsModified())
            {
                //Open a confirmation dialog to check if the user wants to save his changes.
                DialogResult result = openDialog("Do you want to save the changes?");

                if (result == DialogResult.Yes)
                {
                    options.Save();
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
        private DialogResult openDialog(string message)
        {
            DialogViewModelBase vm = new ConfirmationDialogViewModel(message);
            DialogResult result = DialogService.OpenDialog(vm);
            return result;
        }



    }
}
