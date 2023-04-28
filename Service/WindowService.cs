using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Zwiggy.Service
{
    class WindowService
    {
        public static List<AppWindow> AppWindows = new List<AppWindow>();

        public static void AddNewWindow(AppWindow appWindow)
        {
            AppWindows.Add(appWindow);

            AppearanceService.UpdateTitlebar();
        }

        public static void RemoveNewWindow(AppWindow appWindow)
        {
            AppWindows.Remove(appWindow);
        }

        public async static Task CloseAllWindows()
        {
            while(AppWindows.Count > 0)
            {
                await AppWindows[0].CloseAsync();
            }
        }
    }
}
