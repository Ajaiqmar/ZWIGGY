using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Core;
using Zwiggy.AppData;
using Zwiggy.Core.DataManager;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.Usecases.GetDishSearchResults;
using Zwiggy.Core.Usecases.GetRestaurantSearchResults;
using Zwiggy.Core.Utility;
using Zwiggy.View.Contract;
using Zwiggy.ViewModel.Contract;

namespace Zwiggy.ViewModel
{
    class MainViewModel : MainViewModelBase
    {
        private IMainView _view;
        private string _previousRestaurantQuery = null;

        public MainViewModel(IMainView view)
        {
            _view = view;
        }

        private class GetDishSearchResultsPresenterCallback : IPresenterCallback<GetDishSearchResultsResponseObj>
        {
            private MainViewModel _mainVM;

            public GetDishSearchResultsPresenterCallback(MainViewModel mainVM)
            {
                _mainVM = mainVM;
            }

            public void OnError(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            public async Task OnSuccess(GetDishSearchResultsResponseObj responseObj)
            {

                await _mainVM._view.GetDispatcher().RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    
                });
            }
        }

        private class GetRestaurantSearchResultsPresenterCallback : IPresenterCallback<GetRestaurantSearchResultsResponseObj>
        {
            private MainViewModel _mainVM;

            public GetRestaurantSearchResultsPresenterCallback(MainViewModel mainVM)
            {
                _mainVM = mainVM;
            }

            public void OnError(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            public async Task OnSuccess(GetRestaurantSearchResultsResponseObj responseObj)
            {

                await _mainVM._view.GetDispatcher().RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if(responseObj.Restaurants.Count == 0)
                    {
                        _mainVM.IsSearchEmpty = true;
                        _mainVM.IsRestaurantAvailable = false;
                        _mainVM.IsDishAvailable = false;
                    }
                    else
                    {
                        _mainVM.IsSearchEmpty = false;
                        _mainVM.IsRestaurantAvailable = true;
                        _mainVM.IsDishAvailable = false;
                    }

                    _mainVM.Restaurants = new ObservableCollection<RestaurantBObj>(responseObj.Restaurants);
                });
            }
        }

        public override async Task<long> GetCartDishesCountAsync()
        {
            return await FetchCartDataManager.GetCartDishesCountAsync(Repository.CurrentUser);
        }

        public override void GetDishSearchResults()
        {
            
        }

        public override void GetRestaurantSearchResults()
        {
            if(_previousRestaurantQuery != null && _previousRestaurantQuery.Equals(SearchText))
            {
                return;
            }

            GetRestaurantSearchResultsRequestObj requestObj = new GetRestaurantSearchResultsRequestObj()
            {
                SearchQuery = SearchText
            };
            GetRestaurantSearchResults usecase = new GetRestaurantSearchResults(requestObj,new GetRestaurantSearchResultsPresenterCallback(this));
            usecase.Execute();
            _previousRestaurantQuery = SearchText;
        }
    }
}
