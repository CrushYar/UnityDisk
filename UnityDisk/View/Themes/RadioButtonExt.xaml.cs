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
    public sealed class RadioButtonExt:RadioButton
    {
        public RadioButtonExt()
        {
            this.DefaultStyleKey = typeof(RadioButtonExt);
        }

   
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public ContentControl Header
        {
            get => (ContentControl)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
        public ContentControl HeaderChecked
        {
            get => (ContentControl)GetValue(HeaderCheckedProperty);
            set => SetValue(HeaderCheckedProperty, value);
        }

        public static readonly DependencyProperty HeaderProperty =
           DependencyProperty.Register("Header", typeof(ContentControl), typeof(RadioButtonExt), null);
        public static readonly DependencyProperty HeaderCheckedProperty =
                 DependencyProperty.Register("HeaderChecked", typeof(ContentControl), typeof(RadioButtonExt), null);


        private void UpdateControl()
        {
        }

    }
}
