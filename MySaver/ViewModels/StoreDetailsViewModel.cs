using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MySaver.Models;
using MySaver.Services;

namespace MySaver.ViewModels
{
    [QueryProperty("Store", "Store")]
    public partial class StoreDetailsViewModel : BaseViewModel
    {
        IMap map;
        IAlert alert;
        public StoreDetailsViewModel(IMap map, IAlert alert)
        {
            this.map = map;
            this.alert = alert;
        }

        [ObservableProperty]
        Store store;

        [ICommand]
        public async Task OpenMapAsync()
        {
            try
            {
                await map.OpenAsync(Store.Latitude, Store.Longitude, new MapLaunchOptions {Name = Store.Name, NavigationMode = NavigationMode.None});
            }
            catch(Exception ex)
            {
                await alert.DisplayAlert("Error!",
                    $"Unable to open map: {ex.Message}", "OK");
            }
        }
    }

}
