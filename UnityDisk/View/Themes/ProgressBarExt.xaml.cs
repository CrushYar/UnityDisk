using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace UnityDisk.View.Themes
{
    public sealed class ProgressBarExt : ProgressBar
    {
        public ProgressBarExt()
        {
            this.DefaultStyleKey = typeof(ProgressBarExt);
        }

        private Rectangle _progressBarIndicator;
        private Grid _gridStateInfo;
        private Polygon _trianglePolygon;
        private Canvas _canvasStatInfo;
        private bool _infoFlagToLeft;
        private ScaleTransform _scaleTransform;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _progressBarIndicator = GetTemplateChild("ProgressBarIndicator") as Rectangle;
            _gridStateInfo = GetTemplateChild("_gridStateInfo") as Grid;
            _trianglePolygon = GetTemplateChild("_trianglePolygon") as Polygon;
            _canvasStatInfo = GetTemplateChild("_canvasStatInfo") as Canvas;


            _scaleTransform = new ScaleTransform(); ;
            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(_scaleTransform);
            _trianglePolygon.RenderTransform = transformGroup;


            _progressBarIndicator.SizeChanged += (o, e) =>
            {
                var ttv = _progressBarIndicator.TransformToVisual(Window.Current.Content);
                /*    Point screenCoords = ttv.TransformPoint(new Point(0, 0));
                    screenCoords.Y = e.NewSize.Width;*/
                if (_canvasStatInfo.ActualWidth - _progressBarIndicator.ActualWidth < _gridStateInfo.ActualWidth)
                {
                    if (!_infoFlagToLeft)
                    {
                        _infoFlagToLeft = true;
                        _scaleTransform.ScaleX = -1;
                    }
                    _gridStateInfo.SetValue(Canvas.LeftProperty, e.NewSize.Width - _gridStateInfo.ActualWidth);
                }
                else
                {
                    if (_infoFlagToLeft)
                    {
                        _scaleTransform.ScaleX = 1;
                        _infoFlagToLeft = false;
                    }
                    _gridStateInfo.SetValue(Canvas.LeftProperty, e.NewSize.Width);
                }
            };
            UpdateControl();
        }

        public ContentControl Header
        {
            get => (ContentControl)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public double PprogressBarIndicatorHeight
        {
            get => (double)GetValue(PprogressBarIndicatorHeightProperty);
            set => SetValue(PprogressBarIndicatorHeightProperty, value);
        }

        public SolidColorBrush HeaderBackground
        {
            get => (SolidColorBrush)GetValue(HeaderBackgroundProperty);
            set => SetValue(HeaderBackgroundProperty, value);
        }

        public static readonly DependencyProperty PprogressBarIndicatorHeightProperty =
            DependencyProperty.Register("PprogressBarIndicatorHeight", typeof(double), typeof(ProgressBarExt), new PropertyMetadata(10, null));
        public static readonly DependencyProperty HeaderProperty =
           DependencyProperty.Register("Header", typeof(ContentControl), typeof(ProgressBarExt), new PropertyMetadata("value", HeaderPropertyChanged));

        public static readonly DependencyProperty HeaderBackgroundProperty =
                   DependencyProperty.Register("HeaderBackground", typeof(SolidColorBrush), typeof(ProgressBarExt), null);

        private static void HeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textbox = d as ProgressBarExt;
            if (textbox == null || !(e.NewValue is bool))
            {
                return;
            }
        }
       
        private void UpdateControl()
        {
        }

    }
}
