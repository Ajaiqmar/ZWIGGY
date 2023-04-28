using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.Usecases.GetOrderDetails;
using Zwiggy.Service;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Zwiggy.View.User_Control
{
    public sealed partial class OrderDetailsControl : UserControl
    {
        public event EventHandler BackButtonPressed;
        public static readonly DependencyProperty OrderDetailsProperty = DependencyProperty.Register("OrderDetails", typeof(OrderBObj), typeof(OrderDetailsControl), new PropertyMetadata(null));
        public static readonly DependencyProperty DetailsExpandButtonVisibilityProperty = DependencyProperty.Register("DetailsExpandButtonVisibility", typeof(bool), typeof(OrderDetailsControl), new PropertyMetadata(null));
        public static readonly DependencyProperty BackButtonVisibilityProperty = DependencyProperty.Register("BackButtonVisibility", typeof(bool), typeof(OrderDetailsControl), new PropertyMetadata(null));
        public static readonly DependencyProperty OrderDetailsAvailableProperty = DependencyProperty.Register("OrderDetailsAvailable", typeof(bool), typeof(OrderDetailsControl), new PropertyMetadata(null));
        public static readonly DependencyProperty OrderIdProperty = DependencyProperty.Register("OrderId", typeof(string), typeof(OrderDetailsControl), new PropertyMetadata(null));

        private class GetOrderDetailsPresenterCallback : IPresenterCallback<GetOrderDetailsResponseObj>
        {
            private OrderDetailsControl _control;

            public GetOrderDetailsPresenterCallback(OrderDetailsControl control)
            {
                _control = control;
            }

            public void OnError(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            public async Task OnSuccess(GetOrderDetailsResponseObj responseObj)
            {
                CoreDispatcher dispatcher = CoreApplication.MainView.Dispatcher;

                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    _control.OrderDetails = responseObj.OrderObj;
                });
            }
        }

        public OrderBObj OrderDetails
        {
            get { return (OrderBObj)GetValue(OrderDetailsProperty); }
            set { SetValue(OrderDetailsProperty, value); }
        }

        public bool DetailsExpandButtonVisibility
        {
            get { return (bool)GetValue(DetailsExpandButtonVisibilityProperty); }
            set { SetValue(DetailsExpandButtonVisibilityProperty, value); }
        }

        public bool BackButtonVisibility
        {
            get { return (bool)GetValue(BackButtonVisibilityProperty); }
            set { SetValue(BackButtonVisibilityProperty, value); }
        }

        public bool OrderDetailsAvailable
        {
            get { return (bool)GetValue(OrderDetailsAvailableProperty); }
            set { SetValue(OrderDetailsAvailableProperty, value); }
        }

        public string OrderId
        {
            get { return (string)GetValue(OrderIdProperty); }
            set { SetValue(OrderIdProperty, value); }
        }

        public OrderDetailsControl()
        {
            this.InitializeComponent();
        }

        private async void OnNewWindowRequested(object sender, RoutedEventArgs e)
        {
            AppWindow appWindow = await AppWindow.TryCreateAsync();

            WindowService.AddNewWindow(appWindow);

            appWindow.Closed += OnNewWindowClosed;

            OrderDetailsControl detailsControl = new OrderDetailsControl
            {
                OrderDetails = OrderDetails,
                DetailsExpandButtonVisibility = false,
                BackButtonVisibility = false,
                OrderDetailsAvailable = true
            };

            ElementCompositionPreview.SetAppWindowContent(appWindow, detailsControl);

            await appWindow.TryShowAsync();
        }

        private void OnNewWindowClosed(AppWindow app,AppWindowClosedEventArgs e)
        {
            WindowService.RemoveNewWindow(app);
        }

        private async void OnMapLoad(object sender, RoutedEventArgs e)
        {
            GeolocationAccessStatus result = await Geolocator.RequestAccessAsync();

            if (result == GeolocationAccessStatus.Allowed)
            {
                var myLandmarks = new List<MapElement>();

                Geolocator geolocator = new Geolocator();
                Geoposition pos = await geolocator.GetGeopositionAsync();
                Geopoint myLocation = pos.Coordinate.Point;

                BasicGeoposition startLocation = new BasicGeoposition();
                startLocation.Latitude = myLocation.Position.Latitude;
                startLocation.Longitude = myLocation.Position.Longitude;
                Geopoint startPoint = new Geopoint(startLocation);

                BasicGeoposition endLocation = new BasicGeoposition();
                endLocation.Latitude = 12.79587537616805;
                endLocation.Longitude = 80.02106933359201;
                Geopoint endPoint = new Geopoint(endLocation);


                MapRouteFinderResult routeResult = await MapRouteFinder.GetDrivingRouteAsync(
                  startPoint,
                  endPoint,
                  MapRouteOptimization.Distance,
                  MapRouteRestrictions.None,
                  290);

                if (routeResult.Status == MapRouteFinderStatus.Success)
                {
                    MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
                    viewOfRoute.RouteColor = (Application.Current.Resources["ApplicationBaseLightAccentColor"] as SolidColorBrush).Color;
                    viewOfRoute.OutlineColor = Colors.Black;

                    AddressMap.Routes.Add(viewOfRoute);

                    await AddressMap.TrySetViewBoundsAsync(
                      routeResult.Route.BoundingBox,
                      null,
                      MapAnimationKind.Linear);
                }

                var startLocationNeedleIcon = new MapIcon
                {
                    Location = myLocation,
                    NormalizedAnchorPoint = new Point(0.5, 1.0),
                    ZIndex = 0,
                    Title = "Your Location"
                };

                var endLocationNeedleIcon = new MapIcon
                {
                    Location = new Geopoint(endLocation),
                    NormalizedAnchorPoint = new Point(0.5, 1.0),
                    ZIndex = 0,
                    Title = "Delivery Executive's Location"
                };

                myLandmarks.Add(startLocationNeedleIcon);
                myLandmarks.Add(endLocationNeedleIcon);

                var landmarksLayer = new MapElementsLayer
                {
                    ZIndex = 1,
                    MapElements = myLandmarks
                };

                AddressMap.Layers.Add(landmarksLayer);
                AddressMap.LandmarksVisible = true;
            }
        }

        private void OnOrderDetailsPaneSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if((OrderDetailsFirstColumn.MinWidth-20) == CartDetailsGrid.ActualWidth)
            {
                ControlsAdaptiveTrigger.ActivateTrigger(e.NewSize.Width);
            }
            else if(e.NewSize.Width > (ControlsAdaptiveTrigger.MainGridWidth+50) || e.NewSize.Width > 1000)
            {
                ControlsAdaptiveTrigger.DeactivateTrigger();
            }
        }

        private void OnBackButtonPressed(object sender, RoutedEventArgs e)
        {
            BackButtonPressed?.Invoke(sender,new EventArgs());
        }

        private void OnOrderDetailsLoaded(object sender, RoutedEventArgs e)
        {
            if(!OrderDetailsAvailable)
            {
                GetOrderDetailsRequestObj requestObj = new GetOrderDetailsRequestObj()
                {
                    OrderObj = new OrderBObj() { Id = OrderId }
                };

                GetOrderDetails usecase = new GetOrderDetails(new GetOrderDetailsPresenterCallback(this), requestObj);
                usecase.Execute();
            }
        }
    }

    class MyAdaptiveTrigger : StateTriggerBase
    {
        public double MainGridWidth;

        public void ActivateTrigger(double width)
        {
            MainGridWidth = width;
            SetActive(true);
        }

        public void DeactivateTrigger()
        {
            MainGridWidth = 0.0;
            SetActive(false);
        }
    }
}
