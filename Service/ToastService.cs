using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Utility;
using Zwiggy.View;
using Zwiggy.View.User_Control;

namespace Zwiggy.Service
{
    class ToastService
    {
        private static ToastService _Instance = null;

        public static ToastService Instance
        {
            get
            {
                if(_Instance == null)
                {
                    _Instance = new ToastService();
                }

                return _Instance;
            }
        }

        public void ShowTrackOrderToast(OrderBObj OrderObj)
        {
            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "Order has been placed!!"
                            },
                            new AdaptiveText()
                            {
                                Text = "Our delivery executive is on his way to deliver your yummy food."
                            }
                        }
                    }
                },
                Actions = new ToastActionsCustom()
                {
                    Buttons =
                    {
                        new ToastButton("Track Order",OrderObj.Id)
                    }
                }
            };

            var toastNotif = new ToastNotification(toastContent.GetXml());
            toastNotif.Activated += async (sender, args) =>
            {
                ToastActivatedEventArgs eventArgs = args as ToastActivatedEventArgs;

                CoreDispatcher MyDispatcher = CoreApplication.MainView.Dispatcher;

                await MyDispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    AppWindow appWindow = await AppWindow.TryCreateAsync();

                    appWindow.Closed += (app, eArgs) =>
                    {
                        WindowService.RemoveNewWindow(app);
                    };

                    WindowService.AddNewWindow(appWindow);

                    OrderDetailsControl detailsControl = new OrderDetailsControl
                    {
                        OrderDetails = OrderObj,
                        DetailsExpandButtonVisibility = false,
                        BackButtonVisibility = false,
                        OrderDetailsAvailable = true
                    };

                    ElementCompositionPreview.SetAppWindowContent(appWindow, detailsControl);

                    await appWindow.TryShowAsync();
                });
            };

            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }

        public void ShowItemInCartToast(ListBoxItem CartNavigationItem)
        {
            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "Cart is waiting for you !!"
                            }
                        }
                    }
                },
                Launch = "CartView"
            };

            var toastNotif = new ToastNotification(toastContent.GetXml());

            toastNotif.Activated += async (app,args) => {

                ToastActivatedEventArgs eventArgs = args as ToastActivatedEventArgs;

                if (eventArgs.Arguments.Equals("CartView"))
                {
                    CoreDispatcher dispatcher = CoreApplication.MainView.Dispatcher;

                    await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        CartNavigationItem.IsSelected = true;
                    });
                }
            };

            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }

        public void ShowErrorToast(string message)
        {
            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = message
                            }
                        }
                    }
                }
            };

            var toastNotif = new ToastNotification(toastContent.GetXml());

            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }

        public void HandleSuspendedAppActivation(IActivatedEventArgs args)
        {
            ToastNotificationActivatedEventArgs eventArgs = args as ToastNotificationActivatedEventArgs;

            if (eventArgs.Argument.Equals("CartView"))
            {
                Frame rootFrame = Window.Current.Content as Frame;

                if (rootFrame == null)
                {
                    rootFrame = new Frame();
                    Window.Current.Content = rootFrame;
                }

                rootFrame.Navigate(typeof(IndexView), "CartView");

                Window.Current.Activate();
            }
            else if(!eventArgs.Argument.Equals(""))
            {
                Window.Current.Content = new OrderDetailsControl()
                {
                    OrderDetails = null,
                    BackButtonVisibility = false,
                    DetailsExpandButtonVisibility = false,
                    OrderDetailsAvailable = false,
                    OrderId = eventArgs.Argument
                };

                Window.Current.Activate();
            }
        }
    }
}
