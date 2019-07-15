using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TuneMusix.Controls
{
    /// <summary>
    /// Bar chart visualization base class.
    /// </summary>
    [DisplayName("Frquency Visualizer")]
    [Description("Visualizes frequency changes in the signal.")]
    [ToolboxItem(true)]
    abstract class Visualizer : Canvas
    {
        protected const double DEFAULT_SPACING = 2d;
        protected const int DEFAULE_BAR_COUNT = 20;
        protected List<Shape> _bars = new List<Shape>();
        private DispatcherTimer _timer;

        #region Properties

        #region Is Active

        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool),
            typeof(Visualizer), new UIPropertyMetadata(false, OnIsActiveChanged , OnCoerceIsActiveChanged));


        private static object OnCoerceIsActiveChanged(DependencyObject o, object value)
        {
            Visualizer visualizer = o as Visualizer;
            if (visualizer != null)
                return visualizer.OnCoerceIsActiveChanged((bool)value);
            else
                return value;
        }

        protected virtual bool OnCoerceIsActiveChanged(bool value)
        {
            return value;
        }

        private static void OnIsActiveChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Visualizer visualizer = o as Visualizer;
            if(visualizer != null)
            {
                visualizer.OnIsActiveChanged((bool)e.OldValue, (bool)e.NewValue);
            }
        }

        protected virtual void OnIsActiveChanged(bool oldValue, bool newValue)
        {
            if (newValue)
                _timer?.Start();
            else
            {
                _timer?.Stop();
                Clear();
            }
        }

        public bool IsActive
        {
            get
            {
                return (bool)GetValue(IsActiveProperty);
            }
            set
            {              
                SetValue(IsActiveProperty, value);
            }
        }
        #endregion
        #region Spacing


        public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register("Spacing", typeof(double),
            typeof(Visualizer), new UIPropertyMetadata(DEFAULT_SPACING, OnSpacingChanged, OnCoerceSpacingChanged));


        private static object OnCoerceSpacingChanged(DependencyObject o, object value)
        {
            Visualizer visualizer = o as Visualizer;
            if (visualizer != null)
                return visualizer.OnCoerceSpacingChanged((double)value);
            else
                return value;
        }

        protected virtual double OnCoerceSpacingChanged(double value)
        {
            return value;
        }

        private static void OnSpacingChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Visualizer visualizer = o as Visualizer;
            if (visualizer != null)
            {
                visualizer.OnSpacingChanged((double)e.OldValue, (double)e.NewValue);
            }
        }

        protected virtual void OnSpacingChanged(double oldValue, double newValue)
        {

        }

        public double Spacing
        {
            get
            {
                return (double)GetValue(SpacingProperty);
            }
            set
            {
                SetValue(SpacingProperty, value);
            }
        }

        #endregion
        #region Bar Count

        public static readonly DependencyProperty BarCountProperty = DependencyProperty.Register("BarCount", typeof(int),
       typeof(Visualizer), new UIPropertyMetadata(DEFAULE_BAR_COUNT, OnBarCountChanged, OnCoerceBarCountChanged));


        private static object OnCoerceBarCountChanged(DependencyObject o, object value)
        {
            Visualizer visualizer = o as Visualizer;
            if (visualizer != null)
                return visualizer.OnCoerceBarCountChanged((int)value);
            else
                return value;
        }

        protected virtual int OnCoerceBarCountChanged(int value)
        {
            return value;
        }

        private static void OnBarCountChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Visualizer visualizer = o as Visualizer;
            if (visualizer != null)
            {
                visualizer.OnBarCountChanged((int)e.OldValue, (int)e.NewValue);
            }
        }

        protected virtual void OnBarCountChanged(int oldValue, int newValue)
        {

        }

        public int BarCount
        {
            get
            {
                return (int)GetValue(BarCountProperty);
            }
            set
            {
                SetValue(BarCountProperty, value);
                CreateBars();
            }
        }

        #endregion
        #region Bar Values
        public static readonly DependencyProperty BarValuesProperty = DependencyProperty.Register("BarValues", typeof(float[]),
            typeof(Visualizer), new UIPropertyMetadata(new float[DEFAULE_BAR_COUNT], OnBarValuesChanged, OnCoerceBarValues));

        private static object OnCoerceBarValues(DependencyObject o, object value)
        {
            Visualizer visualizer = o as Visualizer;
            if (visualizer != null)
                return visualizer.OnCoerceBarValues((float[])value);
            else
                return value;
        }

        private static void OnBarValuesChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Visualizer visualizer = o as Visualizer;
            if (visualizer != null)
                visualizer.OnBarValuesChanged((float[])e.OldValue, (float[])e.NewValue);
        }

        protected virtual float[] OnCoerceBarValues(float[] value)
        {
            return value;
        }

        protected virtual void OnBarValuesChanged(float[] oldValue, float[] newValue)
        {

        }

        public float[] BarValues
        {
            get
            {
                return (float[])GetValue(BarValuesProperty);
            }
            set
            {
                SetValue(BarValuesProperty, value);
            }
        }

        #endregion
        #region Sample Rate
        public static readonly DependencyProperty SampleRateProperty = DependencyProperty.Register("SampleRate", typeof(int),
            typeof(Visualizer), new UIPropertyMetadata(44100, OnSampleRateChanged, OnCoerceSampleRate));
        private static object OnCoerceSampleRate(DependencyObject o, object value)
        {
            Visualizer visualizer = o as Visualizer;
            if (visualizer != null)
                return visualizer.OnCoerceSampleRate((int)value);
            else
                return value;
        }

        private static void OnSampleRateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Visualizer visualizer = o as Visualizer;
            if (visualizer != null)
                visualizer.OnSampleRateChanged((int)e.OldValue, (int)e.NewValue);
        }

        protected virtual int OnCoerceSampleRate(int value)
        {
            return value;
        }

        protected virtual void OnSampleRateChanged(int oldValue, int newValue)
        {

        }

        public int SampleRate
        {
            get
            {
                return (int)GetValue(SampleRateProperty);
            }
            set
            {
                SetValue(SampleRateProperty, value);
            }
        }
        #endregion
        #region Main Color

        public static readonly DependencyProperty MainColorProperty = DependencyProperty.Register("MainColor", typeof(Brush),
            typeof(Visualizer), new UIPropertyMetadata(Brushes.Blue, OnMainColorChanged, OnCoerceMainColorChanged));


        private static object OnCoerceMainColorChanged(DependencyObject o, object value)
        {
            Visualizer visualizer = o as Visualizer;
            if (visualizer != null)
                return visualizer.OnCoerceMainColorChanged((Brush)value);
            else
                return value;
        }

        protected virtual Brush OnCoerceMainColorChanged(Brush value)
        {
            return value;
        }

        private static void OnMainColorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Visualizer visualizer = o as Visualizer;
            if (visualizer != null)
            {
                visualizer.OnMainColorChanged((Brush)e.OldValue, (Brush)e.NewValue);
            }
        }

        protected virtual void OnMainColorChanged(Brush oldValue, Brush newValue)
        {

        }

        public Brush MainColor
        {
            get
            {
                return (Brush)GetValue(MainColorProperty);
            }
            set
            {
                SetValue(MainColorProperty, value);
            }
        }
        #endregion
        #region Secondary Color

        public static readonly DependencyProperty SecondaryColorProperty = DependencyProperty.Register("SecondaryColor", typeof(Brush),
            typeof(Visualizer), new UIPropertyMetadata(Brushes.Black, OnSecondaryColorChanged, OnCoerceSecondaryColorChanged));


        private static object OnCoerceSecondaryColorChanged(DependencyObject o, object value)
        {
            Visualizer visualizer = o as Visualizer;
            if (visualizer != null)
                return visualizer.OnCoerceSecondaryColorChanged((Brush)value);
            else
                return value;
        }

        protected virtual Brush OnCoerceSecondaryColorChanged(Brush value)
        {
            return value;
        }

        private static void OnSecondaryColorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Visualizer visualizer = o as Visualizer;
            if (visualizer != null)
            {
                visualizer.OnSecondaryColorChanged((Brush)e.OldValue, (Brush)e.NewValue);
            }
        }

        protected virtual void OnSecondaryColorChanged(Brush oldValue, Brush newValue)
        {

        }

        public Brush SecondaryColor
        {
            get
            {
                return (Brush)GetValue(SecondaryColorProperty);
            }
            set
            {
                SetValue(SecondaryColorProperty, value);
            }
        }
        #endregion
        #region Update Rate
        /// <summary>
        /// The update rate of the grahic rendering timer in milliseconds.
        /// </summary>
        public static readonly DependencyProperty UpdateRateProperty = DependencyProperty.Register("UpdateRate", typeof(int),
         typeof(Visualizer), new UIPropertyMetadata(30, OnUpdateRateChanged, OnCoerceUpdateRateChanged));


        private static object OnCoerceUpdateRateChanged(DependencyObject o, object value)
        {
            Visualizer visualizer = o as Visualizer;
            if (visualizer != null)
                return visualizer.OnCoerceUpdateRateChanged((int)value);
            else
                return value;
        }

        protected virtual int OnCoerceUpdateRateChanged(int value)
        {
            return value;
        }

        private static void OnUpdateRateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Visualizer visualizer = o as Visualizer;
            if (visualizer != null)
            {
                visualizer.OnUpdateRateChanged((int)e.OldValue, (int)e.NewValue);
            }
        }

        protected virtual void OnUpdateRateChanged(int oldValue, int newValue)
        {
            _timer.Interval = TimeSpan.FromMilliseconds(newValue);
        }

        public int UpdateRate
        {
            get
            {
                return (int)GetValue(MainColorProperty);
            }
            set
            {
                SetValue(MainColorProperty, value);
            }
        }

        #endregion
        #endregion

        #region Overrides
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            Update();
        }

        public override void OnApplyTemplate()
        {
            this.SizeChanged += onSizeChanged;
        }
        #endregion

        public Visualizer()
        {
            CreateBars();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(30);
            _timer.Tick += onTimerTick;
        }

        /// <summary>
        /// Clears the list of children (shapes).
        /// </summary>
        protected void Clear()
        {
            this.Children.Clear();
        }
        /// <summary>
        /// Creates the bars.
        /// </summary>
        private void CreateBars()
        {
            this.Children.Clear();
            for(int i = 0; i < BarCount; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Fill = MainColor;
                _bars.Add(rect);
            }
            Update();
        }

        #region Events
        private void onTimerTick(object sencer, EventArgs args)
        {
            Update();
        }

        private void onSizeChanged(object sender, EventArgs args)
        {
            Update();
        }
        #endregion

        private void Update()
        {
            if(IsActive)
                UpdateGraphics();
        }

        /// <summary>
        /// Update the height and position of the bars.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected abstract void UpdateGraphics();
    }
}
