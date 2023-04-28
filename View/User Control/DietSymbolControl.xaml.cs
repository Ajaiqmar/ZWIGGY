using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Zwiggy.View.User_Control
{
    public sealed partial class DietSymbolControl : UserControl
    {
        public DietSymbolControl()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty DietColorProperty = DependencyProperty.Register("DietColor",typeof(SolidColorBrush),typeof(DietSymbolControl),new PropertyMetadata(null));


        public SolidColorBrush DietColor
        {
            get { return (SolidColorBrush)GetValue(DietColorProperty); }
            set { SetValue(DietColorProperty, value); }
        }


        public static readonly DependencyProperty DiameterProperty = DependencyProperty.Register("Diameter", typeof(double), typeof(DietSymbolControl), new PropertyMetadata(null));

        public double Diameter
        {
            get { return (double)GetValue(DiameterProperty); }
            set { SetValue(DiameterProperty, value); }
        }
    }
}
