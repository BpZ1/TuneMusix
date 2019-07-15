using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialDesignThemes;
using System.Windows;
using TuneMusix.Helpers;

namespace TuneMusix.ViewModel.Dialog
{
    /// <summary>
    /// ViewModel and therefore datacontext for the warning dialog.
    /// </summary>
    class WarningDialog
    {
        public string Header { get; set; }
        public string Body { get; set; }

        public double Height { get; set; }
        public double Width { get; set; }

        public RelayCommand CloseWindow { get; set; }

        public WarningDialog(string header,string body)
        {
            CloseWindow = new RelayCommand(_closeWindow);
            Height = 300;
            Width = 300;
            Header = header;
            Body = TextFormatService.SpliceText(body,40);
        }

        private void _closeWindow(object argument)
        {
            var window = argument as Window;
            if (window != null)
            {
                window.Close();
            }
        }




    }
}
