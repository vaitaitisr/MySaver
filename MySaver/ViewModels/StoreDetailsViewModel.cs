using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MySaver.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySaver.ViewModels
{
    [QueryProperty("Store", "Store")]
    public partial class StoreDetailsViewModel : BaseViewModel
    {
        IMap map;
        public StoreDetailsViewModel(IMap map)
        {
            this.map = map;
        }

        [ObservableProperty]
        Store store;

        [ICommand]
        async Task OpenMapAsync()
        {
            try
            {
                await map.OpenAsync(Store.Latitude, Store.Longitude, new MapLaunchOptions {Name = Store.Name, NavigationMode = NavigationMode.None});
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!",
                    $"Unable to open map: {ex.Message}", "OK");
            }
        }
    }

}
