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
    [DisplayName("Frquency Visualizer")]
    [Description("Visualizes frequency changes in the signal.")]
    [ToolboxItem(true)]
    class Visualizer : Canvas
    {
        private static readonly double DEFAULT_SPACING = 2d;
        private static readonly int BAR_COUNT = 20;
        private List<Shape> bars = new List<Shape>();
        private DispatcherTimer timer;

        #region Properties

        #region Active

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
        #region Bar Values
        public static readonly DependencyProperty BarValuesProperty = DependencyProperty.Register("BarValues", typeof(float[]),
            typeof(Visualizer), new UIPropertyMetadata(new float[BAR_COUNT], OnBarValuesChanged, OnCoerceBarValues));

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

        #endregion

        #region Overrides
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            updateGraphics();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            updateGraphics();
        }

        public override void OnApplyTemplate()
        {
            this.SizeChanged += onSizeChanged;
        }
        #endregion

        public Visualizer()
        {
            createBars();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(30);
            timer.Tick += onTimerTick;
            timer.Start();
        }

        private void createBars()
        {
            for(int i = 0; i < BAR_COUNT; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Fill = Brushes.Red;
                bars.Add(rect);
            }
        }

        #region Events
        private void onTimerTick(object sencer, EventArgs args)
        {
            updateGraphics();
        }

        private void onSizeChanged(object sender, EventArgs args)
        {
            updateGraphics();
        }
        #endregion

        /// <summary>
        /// Update the height and position of the bars.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void updateGraphics()
        {
            if (!IsActive) return;
 
            if (this.RenderSize.Width < 1 || this.RenderSize.Height < 1) return;

            double canvasHeight = this.ActualHeight;
            double canvasWidth = this.ActualWidth;
            double barWidth = (canvasWidth - (BAR_COUNT * Spacing)) / BAR_COUNT;
            this.Children.Clear();
            int counter = 0;
            double xPosition = Spacing;
            foreach(Shape bar in bars)
            {
                bar.Height = BarValues[counter];
                bar.Width = barWidth;
                Canvas.SetLeft(bar, xPosition);
                Canvas.SetTop(bar, canvasHeight - bar.Height);
                this.Children.Add(bar);
                counter++;
                xPosition += barWidth + Spacing;
            }
        }


    }
}
