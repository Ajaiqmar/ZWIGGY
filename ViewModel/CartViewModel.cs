using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Zwiggy.AppData;
using Zwiggy.Core.DataManager;
using Zwiggy.Core.Model;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Usecases.AddAddress;
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.Usecases.GetAddress;
using Zwiggy.Core.Usecases.GetCart;
using Zwiggy.Core.Usecases.PlaceOrder;
using Zwiggy.Core.Usecases.RemoveFromCart;
using Zwiggy.Core.Usecases.UpdateDishInCart;
using Zwiggy.Core.Utility;
using Zwiggy.Service;
using Zwiggy.View.Contract;
using Zwiggy.View.User_Control;
using Zwiggy.ViewModel.Contract;

namespace Zwiggy.ViewModel
{
    class CartViewModel : CartViewModelBase
    {
        private ToastService _toastManager;
        private ICartView _view;

        public CartViewModel(ICartView view)
        {
            _toastManager = ToastService.Instance;
            _view = view;
        }

        private class AddAddressPresenterCallback : IPresenterCallback<AddAddressResponseObj>
        {
            private CartViewModel _cartVM;

            public AddAddressPresenterCallback(CartViewModel cartVM)
            {
                _cartVM = cartVM;
            }

            public void OnError(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            public async Task OnSuccess(AddAddressResponseObj responseObj)
            {

                await _cartVM._view.GetDispatcher().RunAsync(CoreDispatcherPriority.Normal,() =>
                {
                    _cartVM.Addresses.Add(responseObj.AddressObj);
                    _cartVM.IsAddressSectionEmpty = false;
                });
            }
        }

        private class GetAddressPresenterCallback : IPresenterCallback<GetAddressResponseObj>
        {
            private CartViewModel _cartVM;

            public GetAddressPresenterCallback(CartViewModel cartVM)
            {
                _cartVM = cartVM;
            }

            public void OnError(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            public async Task OnSuccess(GetAddressResponseObj responseObj)
            {
                await _cartVM._view.GetDispatcher().RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    _cartVM.Addresses = new ObservableCollection<Address>(responseObj.Addresses);

                    if (_cartVM.Addresses.Count == 0)
                    {
                        _cartVM.IsAddressSectionEmpty = true;
                    }
                    else
                    {
                        _cartVM.IsAddressSectionEmpty = false;
                    }
                });
            }
        }

        private class GetCartPresenterCallback : IPresenterCallback<GetCartResponseObj>
        {
            private CartViewModel _cartVM;

            public GetCartPresenterCallback(CartViewModel cartVM)
            {
                _cartVM = cartVM;
            }

            public void OnError(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            public async Task OnSuccess(GetCartResponseObj responseObj)
            {
                await _cartVM._view.GetDispatcher().RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    _cartVM.CartObj = responseObj.Cart;

                    if(_cartVM.CartObj.Dishes.Count != 0)
                    {
                        _cartVM.IsCartEmpty = false;
                    }
                    else
                    {
                        _cartVM.IsCartEmpty = true;
                    }
                });
            }
        }

        private class RemoveFromCartPresenterCallback : IPresenterCallback<RemoveFromCartResponseObj>
        {
            private CartViewModel _cartVM;

            public RemoveFromCartPresenterCallback(CartViewModel cartVM)
            {
                _cartVM = cartVM;
            }

            public void OnError(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            public async Task OnSuccess(RemoveFromCartResponseObj responseObj)
            {
                await _cartVM._view.GetDispatcher().RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    for(int i=0;i<_cartVM.CartObj.Dishes.Count;i++)
                    {
                        if(_cartVM.CartObj.Dishes[i].Id.Equals(responseObj.DishObj.Id))
                        {
                            _cartVM.CartObj.Dishes.RemoveAt(i);
                            break;
                        }
                    }

                    if(_cartVM.CartObj.Dishes.Count == 0)
                    {
                        _cartVM.IsCartEmpty = true;
                    }
                });
            }
        }
        
        private class UpdateDishInCartPresenterCallback : IPresenterCallback<UpdateDishInCartResponseObj>
        {
            private CartViewModel _cartVM;

            public UpdateDishInCartPresenterCallback(CartViewModel cartVM)
            {
                _cartVM = cartVM;
            }

            public void OnError(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            public async Task OnSuccess(UpdateDishInCartResponseObj responseObj)
            {
                await _cartVM._view.GetDispatcher().RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    for (int i = 0; i < _cartVM.CartObj.Dishes.Count; i++)
                    {
                        if (_cartVM.CartObj.Dishes[i].Id.Equals(responseObj.DishObj.Id))
                        {
                            _cartVM.CartObj.Dishes[i].DishCount = responseObj.DishObj.DishCount;
                        }
                    }
                });
            }
        }

        private class PlaceOrderPresenterCallback : IPresenterCallback<PlaceOrderResponseObj>
        {
            private CartViewModel _cartVM;

            public PlaceOrderPresenterCallback(CartViewModel cartVM)
            {
                _cartVM = cartVM;
            }

            public void OnError(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            public async Task OnSuccess(PlaceOrderResponseObj responseObj)
            {
                await _cartVM._view.GetDispatcher().RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    _cartVM._toastManager.ShowTrackOrderToast(responseObj.OrderObj);
                });
            }
        }

        public override void AddAddress()
        {
            AddAddressRequestObj requestObj = new AddAddressRequestObj() 
            {
                AddressObj = new Address()
                {
                    Id = Guid.NewGuid().ToString(),
                    AddressDescription = AddressDescription,
                    Nickname = Nickname,
                    UserId = Repository.CurrentUser.Email
                }
            };

            AddAddress usecase = new AddAddress(requestObj,new AddAddressPresenterCallback(this));
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

        public override void GetCart()
        {
            GetCartRequestObj requestObj = new GetCartRequestObj() { UserObj = Repository.CurrentUser };
            GetCart usecase = new GetCart(new GetCartPresenterCallback(this), requestObj);

            usecase.Execute();
        }

        public override void AddToCart(DishBObj dish)
        {
            dish.DishCount += 1;
            CartObj.BillAmount += dish.Cost;
            TotalPay = CartObj.BillAmount;

            UpdateDishInCartRequestObj requestObj = new UpdateDishInCartRequestObj()
            {
                DishObj = dish,
                UserObj = Repository.CurrentUser
            };

            UpdateDishInCart usecase = new UpdateDishInCart(new UpdateDishInCartPresenterCallback(this),requestObj);
            usecase.Execute();
        }

        public override void RemoveFromCart(DishBObj dish)
        {
            dish.DishCount -= 1;
            CartObj.BillAmount -= dish.Cost;
            TotalPay = CartObj.BillAmount;

            if (dish.DishCount == 0)
            {
                RemoveFromCartRequestObj requestObj = new RemoveFromCartRequestObj()
                {
                    DishObj = dish,
                    UserObj = Repository.CurrentUser
                };

                RemoveFromCart usecase = new RemoveFromCart(new RemoveFromCartPresenterCallback(this),requestObj);
                usecase.Execute();
            }
            else
            {
                UpdateDishInCartRequestObj requestObj = new UpdateDishInCartRequestObj()
                {
                    DishObj = dish,
                    UserObj = Repository.CurrentUser
                };

                UpdateDishInCart usecase = new UpdateDishInCart(new UpdateDishInCartPresenterCallback(this), requestObj);
                usecase.Execute();
            }
        }

        public override void PlaceOrder()
        {
            PlaceOrderRequestObj requestObj = new PlaceOrderRequestObj
            {
                OrderObj = new OrderBObj
                {
                    Id = Guid.NewGuid().ToString(),
                    AddressPicked = AddressSelected,
                    ModeOfPayment = ModeOfPayment,
                    UserId = Repository.CurrentUser.Email,
                    Dishes = CartObj.Dishes,
                    OrderPlacedDateAndTime = DateTime.Now,
                    IsDelivered = false,
                    RestaurantPicked = CartObj.RestaurantPicked
                },
                UserObj = Repository.CurrentUser
            };

            PlaceOrder usecase = new PlaceOrder(new PlaceOrderPresenterCallback(this),requestObj);
            usecase.Execute();
        }
    }
}
