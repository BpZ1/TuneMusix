using System.Windows;

namespace TuneMusix.Helpers.Util
{
    /// <summary>
    /// Proxy class for binding controls to out of reach data context.
    /// </summary>
    class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy),
            new UIPropertyMetadata(null));
    }
}
