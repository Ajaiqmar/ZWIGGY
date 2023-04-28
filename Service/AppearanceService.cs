using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Zwiggy.Service
{
    class AppearanceService
    {
        public static void UpdateTitlebar()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            titleBar.BackgroundColor = (Application.Current.Resources["ApplicationBaseAccentColor"] as SolidColorBrush).Color;
            titleBar.ButtonBackgroundColor = (Application.Current.Resources["ApplicationBaseAccentColor"] as SolidColorBrush).Color;
            titleBar.ButtonInactiveBackgroundColor = (Application.Current.Resources["ApplicationBaseAccentColor"] as SolidColorBrush).Color;
            titleBar.InactiveBackgroundColor = (Application.Current.Resources["ApplicationBaseAccentColor"] as SolidColorBrush).Color;
            titleBar.InactiveForegroundColor = Colors.White;
            titleBar.ButtonInactiveForegroundColor = Colors.White;
            titleBar.ButtonForegroundColor = Colors.White;
            titleBar.ForegroundColor = Colors.White;
            titleBar.ButtonHoverBackgroundColor = (Application.Current.Resources["ApplicationBaseHoverAccentColor"] as SolidColorBrush).Color;
            titleBar.ButtonHoverForegroundColor = Colors.White;
            titleBar.ButtonPressedBackgroundColor = (Application.Current.Resources["ApplicationBaseHoverAccentColor"] as SolidColorBrush).Color;
            titleBar.ButtonPressedForegroundColor = Colors.White;

            foreach(AppWindow appWindow in WindowService.AppWindows)
            {
                var appWindowTitleBar = appWindow.TitleBar;

                appWindowTitleBar.BackgroundColor = (Application.Current.Resources["ApplicationBaseAccentColor"] as SolidColorBrush).Color;
                appWindowTitleBar.ButtonBackgroundColor = (Application.Current.Resources["ApplicationBaseAccentColor"] as SolidColorBrush).Color;
                appWindowTitleBar.ButtonInactiveBackgroundColor = (Application.Current.Resources["ApplicationBaseAccentColor"] as SolidColorBrush).Color;
                appWindowTitleBar.InactiveBackgroundColor = (Application.Current.Resources["ApplicationBaseAccentColor"] as SolidColorBrush).Color;
                appWindowTitleBar.InactiveForegroundColor = Colors.White;
                appWindowTitleBar.ButtonInactiveForegroundColor = Colors.White;
                appWindowTitleBar.ButtonForegroundColor = Colors.White;
                appWindowTitleBar.ForegroundColor = Colors.White;
                appWindowTitleBar.ButtonHoverBackgroundColor = (Application.Current.Resources["ApplicationBaseHoverAccentColor"] as SolidColorBrush).Color;
                appWindowTitleBar.ButtonHoverForegroundColor = Colors.White;
                appWindowTitleBar.ButtonPressedBackgroundColor = (Application.Current.Resources["ApplicationBaseHoverAccentColor"] as SolidColorBrush).Color;
                appWindowTitleBar.ButtonPressedForegroundColor = Colors.White;
            }
        }

        public static void UpdateTitlebarForIndexView()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            titleBar.BackgroundColor = Colors.DarkOrange;
            titleBar.ButtonBackgroundColor = Colors.DarkOrange;
            titleBar.ButtonInactiveBackgroundColor = Colors.DarkOrange;
            titleBar.InactiveBackgroundColor = Colors.DarkOrange;
            titleBar.InactiveForegroundColor = Colors.White;
            titleBar.ButtonInactiveForegroundColor = Colors.White;
            titleBar.ButtonForegroundColor = Colors.White;
            titleBar.ForegroundColor = Colors.White;
            titleBar.ButtonHoverBackgroundColor = Color.FromArgb(255, 242, 133, 0);
            titleBar.ButtonHoverForegroundColor = Colors.White;
            titleBar.ButtonPressedBackgroundColor = Color.FromArgb(255, 242, 133, 0);
            titleBar.ButtonPressedForegroundColor = Colors.White;

            foreach (AppWindow appWindow in WindowService.AppWindows)
            {
                var appWindowTitleBar = appWindow.TitleBar;

                appWindowTitleBar.BackgroundColor = Colors.DarkOrange;
                appWindowTitleBar.ButtonBackgroundColor = Colors.DarkOrange;
                appWindowTitleBar.ButtonInactiveBackgroundColor = Colors.DarkOrange;
                appWindowTitleBar.InactiveBackgroundColor = Colors.DarkOrange;
                appWindowTitleBar.InactiveForegroundColor = Colors.White;
                appWindowTitleBar.ButtonInactiveForegroundColor = Colors.White;
                appWindowTitleBar.ButtonForegroundColor = Colors.White;
                appWindowTitleBar.ForegroundColor = Colors.White;
                appWindowTitleBar.ButtonHoverBackgroundColor = Color.FromArgb(255, 242, 133, 0);
                appWindowTitleBar.ButtonHoverForegroundColor = Colors.White;
                appWindowTitleBar.ButtonPressedBackgroundColor = Color.FromArgb(255, 242, 133, 0);
                appWindowTitleBar.ButtonPressedForegroundColor = Colors.White;
            }
        }

        public static void ChangeAccentColor(string type)
        {
            switch (type)
            {
                case "DarkOrangeGrid":
                    (Application.Current.Resources["ApplicationBaseAccentColor"] as SolidColorBrush).Color = Colors.DarkOrange;
                    (Application.Current.Resources["ApplicationBaseHoverAccentColor"] as SolidColorBrush).Color = Color.FromArgb(255, 242, 133, 0);
                    (Application.Current.Resources["ApplicationBaseLightAccentColor"] as SolidColorBrush).Color = Color.FromArgb(255, 255, 222, 173);
                    break;
                case "SystemColorGrid":
                    (Application.Current.Resources["ApplicationBaseAccentColor"] as SolidColorBrush).Color = (Color)Application.Current.Resources["SystemAccentColor"];
                    (Application.Current.Resources["ApplicationBaseHoverAccentColor"] as SolidColorBrush).Color = Color.FromArgb(255, 0, 108, 194);
                    (Application.Current.Resources["ApplicationBaseLightAccentColor"] as SolidColorBrush).Color = Color.FromArgb(255, 179, 216, 255);
                    break;
                case "ChocolateGrid":
                    (Application.Current.Resources["ApplicationBaseAccentColor"] as SolidColorBrush).Color = Colors.Chocolate;
                    (Application.Current.Resources["ApplicationBaseHoverAccentColor"] as SolidColorBrush).Color = Color.FromArgb(255, 189, 95, 27);
                    (Application.Current.Resources["ApplicationBaseLightAccentColor"] as SolidColorBrush).Color = Color.FromArgb(255, 255, 199, 160);
                    break;
                case "GreenGrid":
                    (Application.Current.Resources["ApplicationBaseAccentColor"] as SolidColorBrush).Color = Colors.Green;
                    (Application.Current.Resources["ApplicationBaseHoverAccentColor"] as SolidColorBrush).Color = Color.FromArgb(255, 0, 115, 0);
                    (Application.Current.Resources["ApplicationBaseLightAccentColor"] as SolidColorBrush).Color = Color.FromArgb(255, 158, 222, 158);
                    break;
                case "TomatoGrid":
                    (Application.Current.Resources["ApplicationBaseAccentColor"] as SolidColorBrush).Color = Colors.Tomato;
                    (Application.Current.Resources["ApplicationBaseHoverAccentColor"] as SolidColorBrush).Color = Color.FromArgb(255, 230, 89, 64);
                    (Application.Current.Resources["ApplicationBaseLightAccentColor"] as SolidColorBrush).Color = Color.FromArgb(255, 255, 176, 162);
                    break;
                case "PaleVioletRedGrid":
                    (Application.Current.Resources["ApplicationBaseAccentColor"] as SolidColorBrush).Color = Colors.PaleVioletRed;
                    (Application.Current.Resources["ApplicationBaseHoverAccentColor"] as SolidColorBrush).Color = Color.FromArgb(255, 198, 101, 133);
                    (Application.Current.Resources["ApplicationBaseLightAccentColor"] as SolidColorBrush).Color = Color.FromArgb(255, 255, 183, 207);
                    break;
                case "GoldenRodGrid":
                    (Application.Current.Resources["ApplicationBaseAccentColor"] as SolidColorBrush).Color = Colors.Goldenrod;
                    (Application.Current.Resources["ApplicationBaseHoverAccentColor"] as SolidColorBrush).Color = Color.FromArgb(255, 197, 149, 29);
                    (Application.Current.Resources["ApplicationBaseLightAccentColor"] as SolidColorBrush).Color = Color.FromArgb(255, 255, 232, 177);
                    break;
            }

            ApplicationData.Current.LocalSettings.Values["AccentColorType"] = type;
            UpdateTitlebar();
        }
    }
}
