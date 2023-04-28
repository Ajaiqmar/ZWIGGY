using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Zwiggy.View.Templated_Control
{
    public sealed class CustomFlipView : FlipView
    {
        public CustomFlipView()
        {
            this.DefaultStyleKey = typeof(CustomFlipView);
        }

        protected override void OnApplyTemplate()
        {
            this.SelectionChanged += (s, e) =>
            {
                UpdateView();
            };

            Button button = GetTemplateChild("MyPreviousButtonHorizontal") as Button;
            button.Foreground = new SolidColorBrush(Colors.Black)
            {
                Opacity = 0.3
            };

            button.Click += (s, e) =>
            {
                if(SelectedIndex > 0)
                {
                    SelectedIndex -= 1;
                }
            };

            button = GetTemplateChild("MyNextButtonHorizontal") as Button;

            button.Click += (s, e) =>
            {
                if (SelectedIndex < (Items.Count - 1))
                {
                    SelectedIndex += 1;
                }
            };
        }

        public void UpdateView()
        {
            if(SelectedIndex == 0)
            {
                Button button = GetTemplateChild("MyPreviousButtonHorizontal") as Button;
                button.Foreground = new SolidColorBrush(Colors.Black)
                {
                    Opacity = 0.3
                };
            }
            else if(SelectedIndex == (Items.Count - 1))
            {
                Button button = GetTemplateChild("MyNextButtonHorizontal") as Button;
                button.Foreground = new SolidColorBrush(Colors.Black)
                {
                    Opacity = 0.3
                };
            }
            else
            {
                Button button = GetTemplateChild("MyPreviousButtonHorizontal") as Button;
                button.Foreground = new SolidColorBrush(Colors.Black);
                button = GetTemplateChild("MyNextButtonHorizontal") as Button;
                button.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
    }
}
