using System;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TuneMusix.Helpers;

namespace TuneMusix.Controls
{
    /// <summary>
    /// Slider that displays the tooltip value in time format.
    /// </summary>
    public partial class FormattedSlider : Slider
    {
        private ToolTip _autoToolTip;
        private string _autoToolTipFormat;
        private bool _isTimeValue;

        /// <summary>
        /// Gets/sets a format string used to modify the auto tooltip's content.
        /// Note: This format string must contain exactly one placeholder value,
        /// which is used to hold the tooltip's original content.
        /// </summary>
        public string AutoToolTipFormat
        {
            get { return _autoToolTipFormat; }
            set { _autoToolTipFormat = value; }
        }

        public bool IsTimeValue
        {
            get { return _isTimeValue; }
            set { _isTimeValue = value; }
        }


        protected override void OnThumbDragStarted(DragStartedEventArgs e)
        {
            base.OnThumbDragStarted(e);
            this.FormatAutoToolTipContent();
        }

        protected override void OnThumbDragDelta(DragDeltaEventArgs e)
        {
            base.OnThumbDragDelta(e);
            this.FormatAutoToolTipContent();
        }

        private void FormatAutoToolTipContent()
        {
            //Format the sting using the formater.
            if (!string.IsNullOrEmpty(this.AutoToolTipFormat))
            {
                this.AutoToolTip.Content = string.Format(
                    this.AutoToolTipFormat,
                    this.AutoToolTip.Content);
            }
            //Format the string if it is a time value in seconds.
            if (IsTimeValue)
            {

                this.AutoToolTip.Content = Converter.TimeSpanToString(TimeSpan.FromSeconds(
                    Double.Parse((string)this.AutoToolTip.Content)));
            }
        }


        private System.Windows.Controls.ToolTip AutoToolTip
        {
            get
            {
                if (_autoToolTip == null)
                {
                    FieldInfo field = typeof(Slider).GetField(
                        "_autoToolTip",
                        BindingFlags.NonPublic | BindingFlags.Instance);

                    _autoToolTip = field.GetValue(this) as System.Windows.Controls.ToolTip;
                }

                return _autoToolTip;
            }
        }
    }
}
