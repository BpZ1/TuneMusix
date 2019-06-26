using System.Windows.Controls;
using System.Windows.Shapes;

namespace TuneMusix.Controls
{
    /// <summary>
    /// Standard bar chart representation.
    /// </summary>
    class FlatVisualization : Visualizer
    {

        public FlatVisualization() : base() { }

        protected override void UpdateGraphics()
        {

            if (this.RenderSize.Width < 1 || this.RenderSize.Height < 1) return;
            
            double canvasWidth = this.ActualWidth;
            double canvasHeight = this.ActualHeight;
            //Represents one percent of the height
            double heightUnit = canvasHeight / 100;

            double barWidth = (canvasWidth - (BarCount * Spacing)) / BarCount;
            this.Children.Clear();
            int counter = 0;
            double xPosition = Spacing;
            foreach (Shape bar in _bars)
            {
                bar.Height = BarValues[counter] * heightUnit;
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
