using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TuneMusix.Helpers;
using TuneMusix.Helpers.Dialogs;

namespace TuneMusix.ViewModel.Dialog
{
    /// <summary>
    /// ViewModel for the confirmation dialog.
    /// </summary>
    class ConfirmationDialogViewModel : DialogViewModelBase
    {
        private string messageBox = "";
        private bool buttonPressed = false;

        public ConfirmationDialogViewModel()
        {
            yesCommand = new RelayCommand(onYesClick);
            noCommand = new RelayCommand(onNoClick);
            cancelCommand = new RelayCommand(onCancelClick);
            exitButtonCommand = new RelayCommand(onExitClick);
        }

        public ConfirmationDialogViewModel(string message)
        {
            messageBox = message;
            yesCommand = new RelayCommand(onYesClick);
            noCommand = new RelayCommand(onNoClick);
            cancelCommand = new RelayCommand(onCancelClick);
            exitButtonCommand = new RelayCommand(onExitClick);
        }

        private ICommand yesCommand = null;
        public ICommand YesCommand
        {
            get { return yesCommand; }
            set { yesCommand = value; }
        }

        private ICommand noCommand = null;
        public ICommand NoCommand
        {
            get { return noCommand; }
            set { noCommand = value; }
        }

        private ICommand cancelCommand = null;
        public ICommand CancelCommand
        {
            get { return cancelCommand; }
            set { cancelCommand = value; }
        }
        private ICommand exitButtonCommand = null;
        public ICommand ExitButtonCommand
        {
            get { return exitButtonCommand; }
            set { exitButtonCommand = value; }
        }

        public string MessageBox
        {
            get { return messageBox; }
            set { messageBox = value; }
        }

        private void onYesClick(object parameter)
        {
            if (!buttonPressed)
            {
                buttonPressed = true;
                this.CloseDialogWithResult(parameter as Window, DialogResult.Yes);
            }  
        }

        private void onNoClick(object parameter)
        {
            if (!buttonPressed)
            {
                buttonPressed = true;
                this.CloseDialogWithResult(parameter as Window, DialogResult.No);
            }

        }

        private void onCancelClick(object parameter)
        {
            if (!buttonPressed)
            {
                buttonPressed = true;
                this.CloseDialogWithResult(parameter as Window, DialogResult.Undefined);
            }   
        }
        
        private void onExitClick(object parameter)
        {
            if (!buttonPressed)
                this.CloseDialogWithResult(parameter as Window, DialogResult.Undefined);
        }
    }
}
