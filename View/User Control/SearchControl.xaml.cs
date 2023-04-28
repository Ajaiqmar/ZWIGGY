using System;
using System.Diagnostics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Zwiggy.View.User_Control
{
    public sealed partial class SearchControl : UserControl
    {
        public event EventHandler TextboxGotFocus;
        public event EventHandler TextboxLostFocus;
        public event EventHandler SearchButtonPressed;

        public SearchControl()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register("PlaceholderText",typeof(string),typeof(SearchControl),new PropertyMetadata(null));

        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value+""); }
        }

        public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register("SearchText", typeof(string), typeof(SearchControl), new PropertyMetadata(null));

        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value + ""); }
        }

        public static readonly DependencyProperty PlaceholderForegroundProperty = DependencyProperty.Register("PlaceholderForeground", typeof(SolidColorBrush), typeof(SearchControl), new PropertyMetadata(null));

        public SolidColorBrush PlaceholderForeground
        {
            get { 
                return (SolidColorBrush)GetValue(PlaceholderForegroundProperty); }
            set { SetValue(PlaceholderForegroundProperty, value); }
        }

        private void OnTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.PlaceholderForeground = new SolidColorBrush(Colors.Black);
            textBox.Foreground = new SolidColorBrush(Colors.Black);

            TextboxGotFocus?.Invoke(sender,new EventArgs());
        }

        private void OnTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.PlaceholderForeground = PlaceholderForeground;
            textBox.Foreground = PlaceholderForeground;

            TextboxLostFocus?.Invoke(sender, new EventArgs());
        }

        private void OnButtonLoaded(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            button.Resources["ButtonForegroundPressed"] = PlaceholderForeground;
            button.Resources["ButtonForegroundPointerOver"] = PlaceholderForeground;
        }

        private void OnTextBoxLoaded(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.Resources["TextControlForegroundPointerOver"] = PlaceholderForeground;
        }

        private void OnTextBoxFocusEngaged(Control sender, FocusEngagedEventArgs args)
        {
            Debug.WriteLine("Hello");
        }

        private void OnSearchButtonClicked(object sender, RoutedEventArgs e)
        {
            SearchButtonPressed?.Invoke(sender, new EventArgs());
        }
    }
}
