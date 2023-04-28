using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Zwiggy.AppData;
using Zwiggy.Core.Model;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.Usecases.GetAddress;
using Zwiggy.Core.Usecases.GetFavouriteRestaurants;
using Zwiggy.Core.Usecases.GetOrderDetails;
using Zwiggy.Core.Usecases.GetOrders;
using Zwiggy.Core.Usecases.RemoveAddress;
using Zwiggy.Service;
using Zwiggy.View.Contract;
using Zwiggy.ViewModel.Contract;
using Zwiggy.ViewObj;

namespace Zwiggy.ViewModel
{
    class ProfileViewModel : ProfileViewModelBase
    {
        private IProfileView _view;

        public ProfileViewModel(IProfileView view)
        {
            _view = view;
        }

        private class GetFavouriteRestaurantsPresenterCallback : IPresenterCallback<GetFavouriteRestaurantsResponseObj>
        {
            private ProfileViewModel _profileVM;

            public GetFavouriteRestaurantsPresenterCallback(ProfileViewModel profileVM)
            {
                _profileVM = profileVM;
            }

            public void OnError(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            public async Task OnSuccess(GetFavouriteRestaurantsResponseObj responseObj)
            {
                await _profileVM._view.GetDispatcher().RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    _profileVM.Restaurants = new ObservableCollection<RestaurantBObj>(responseObj.Restaurants);

                    _profileVM.IsFavouritesEmpty = false;
                    _profileVM.IsFavouritesNotEmpty = false;
                    _profileVM.IsOrderEmpty = false;
                    _profileVM.IsOrderNotEmpty = false;
                    _profileVM.IsAddressEmpty = false;
                    _profileVM.IsAddressNotEmpty = false;
                    _profileVM.IsSettingsVisible = false;

                    if (_profileVM.Restaurants.Count == 0)
                    {
                        _profileVM.IsFavouritesEmpty = true;
                    }
                    else
                    {
                        _profileVM.IsFavouritesNotEmpty = true;
                    }
                });
            }
        }

        private class GetAddressPresenterCallback : IPresenterCallback<GetAddressResponseObj>
        {
            private ProfileViewModel _profileVM;

            public GetAddressPresenterCallback(ProfileViewModel profileVM)
            {
                _profileVM = profileVM;
            }

            public void OnError(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            public async Task OnSuccess(GetAddressResponseObj responseObj)
            {
                await _profileVM._view.GetDispatcher().RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    _profileVM.Addresses = new ObservableCollection<Address>(responseObj.Addresses);

                    _profileVM.IsFavouritesEmpty = false;
                    _profileVM.IsFavouritesNotEmpty = false;
                    _profileVM.IsOrderEmpty = false;
                    _profileVM.IsOrderNotEmpty = false;
                    _profileVM.IsAddressEmpty = false;
                    _profileVM.IsAddressNotEmpty = false;
                    _profileVM.IsSettingsVisible = false;

                    if (_profileVM.Addresses.Count == 0)
                    {
                        _profileVM.IsAddressEmpty = true;
                    }
                    else
                    {
                        _profileVM.IsAddressNotEmpty = true;
                    }
                });
            }
        }

        private class RemoveAddressPresenterCallback : IPresenterCallback<RemoveAddressResponseObj>
        {
            private ProfileViewModel _profileVM;

            public RemoveAddressPresenterCallback(ProfileViewModel profileVM)
            {
                _profileVM = profileVM;
            }

            public void OnError(Exception ex)
            {
                ToastService.Instance.ShowErrorToast("There seems to be an error in removing the address");
            }

            public async Task OnSuccess(RemoveAddressResponseObj responseObj)
            {
                await _profileVM._view.GetDispatcher().RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    _profileVM.Addresses.Remove(responseObj.AddressObj);
                });
            }
        }

        private class GetOrdersPresenterCallback : IPresenterCallback<GetOrdersResponseObj>
        {
            private ProfileViewModel _profileVM;

            public GetOrdersPresenterCallback(ProfileViewModel profileVM)
            {
                _profileVM = profileVM;
            }

            public void OnError(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            public async Task OnSuccess(GetOrdersResponseObj responseObj)
            {
                await _profileVM._view.GetDispatcher().RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    _profileVM.Orders = new ObservableCollection<OrderBObj>(responseObj.Orders);
                    _profileVM.IsFavouritesEmpty = false;
                    _profileVM.IsFavouritesNotEmpty = false;
                    _profileVM.IsOrderEmpty = false;
                    _profileVM.IsOrderNotEmpty = false;
                    _profileVM.IsAddressEmpty = false;
                    _profileVM.IsAddressNotEmpty = false;
                    _profileVM.IsSettingsVisible = false;

                    if (_profileVM.Orders.Count == 0)
                    {
                        _profileVM.IsOrderEmpty = true;
                    }
                    else
                    {
                        _profileVM.IsOrderNotEmpty = true;
                    }
                });
            }
        }

        private class GetOrderDetailsPresenterCallback : IPresenterCallback<GetOrderDetailsResponseObj>
        {
            private ProfileViewModel _profileVM;

            public GetOrderDetailsPresenterCallback(ProfileViewModel profileVM)
            {
                _profileVM = profileVM;
            }

            public void OnError(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            public async Task OnSuccess(GetOrderDetailsResponseObj responseObj)
            {
                await _profileVM._view.GetDispatcher().RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    _profileVM.OrderDetails = responseObj.OrderObj;
                });
            }
        }

        public ProfileViewModel()
        {
            IList<ProfileSections> sections = new List<ProfileSections>();

            sections.Add(new ProfileSections() 
            { 
                Name="Orders",
                IconPath= "../Assets/Images/Icons/order.png"
            });
            sections.Add(new ProfileSections() 
            { 
                Name="Favourites",
                IconPath= "../Assets/Images/Icons/favourites.png"
            });
            sections.Add(new ProfileSections() 
            { 
                Name="Adresses",
                IconPath= "../Assets/Images/Icons/location.png"
            });
            sections.Add(new ProfileSections() 
            { 
                Name="Settings",
                IconPath= "../Assets/Images/Icons/settings.png"
            });

            Sections = sections;
        }

        public override void GetFavouriteRestaurants()
        {
            GetFavouriteRestaurantsRequestObj requestObj = new GetFavouriteRestaurantsRequestObj()
            {
                UserObj = Repository.CurrentUser
            };

            GetFavouriteRestaurants usecase = new GetFavouriteRestaurants(requestObj,new GetFavouriteRestaurantsPresenterCallback(this));
            usecase.Execute();
        }

        public override void GetAddress()
        {
            GetAddressRequestObj requestObj = new GetAddressRequestObj()
            {
                UserObj = Repository.CurrentUser
            };

            GetAddress usecase = new GetAddress(requestObj,new GetAddressPresenterCallback(this));
            usecase.Execute();
        }

        public override void RemoveAddress(Address address)
        {
            RemoveAddressRequestObj requestObj = new RemoveAddressRequestObj()
            {
                AddressObj = address
            };

            RemoveAddress usecase = new RemoveAddress(requestObj,new RemoveAddressPresenterCallback(this));
            usecase.Execute();
        }

        public override void GetOrders()
        {
            GetOrdersRequestObj requestObj = new GetOrdersRequestObj()
            {
                UserObj = Repository.CurrentUser
            };

            GetOrders usecase = new GetOrders(new GetOrdersPresenterCallback(this), requestObj);
            usecase.Execute();
        }

        public override void GetOrderDetails(OrderBObj order)
        {
            GetOrderDetailsRequestObj requestObj = new GetOrderDetailsRequestObj()
            {
                OrderObj = order
            };

            GetOrderDetails usecase = new GetOrderDetails(new GetOrderDetailsPresenterCallback(this), requestObj);
            usecase.Execute();
        }
    }
}
