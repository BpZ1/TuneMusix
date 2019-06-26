using System.Windows;
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
        private string _messageBox = "";
        private bool _buttonPressed = false;

        public ConfirmationDialogViewModel()
        {
            yesCommand = new RelayCommand(OnYesClick);
            noCommand = new RelayCommand(OnNoClick);
            cancelCommand = new RelayCommand(OnCancelClick);
            exitButtonCommand = new RelayCommand(OnExitClick);
        }

        public ConfirmationDialogViewModel(string message)
        {
            _messageBox = message;
            yesCommand = new RelayCommand(OnYesClick);
            noCommand = new RelayCommand(OnNoClick);
            cancelCommand = new RelayCommand(OnCancelClick);
            exitButtonCommand = new RelayCommand(OnExitClick);
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
            get { return _messageBox; }
            set { _messageBox = value; }
        }

        private void OnYesClick(object parameter)
        {
            if (!_buttonPressed)
            {
                _buttonPressed = true;
                this.CloseDialogWithResult(parameter as Window, DialogResult.Yes);
            }  
        }

        private void OnNoClick(object parameter)
        {
            if (!_buttonPressed)
            {
                _buttonPressed = true;
                this.CloseDialogWithResult(parameter as Window, DialogResult.No);
            }

        }

        private void OnCancelClick(object parameter)
        {
            if (!_buttonPressed)
            {
                _buttonPressed = true;
                this.CloseDialogWithResult(parameter as Window, DialogResult.Undefined);
            }   
        }
        
        private void OnExitClick(object parameter)
        {
            if (!_buttonPressed)
                this.CloseDialogWithResult(parameter as Window, DialogResult.Undefined);
        }
    }
}
