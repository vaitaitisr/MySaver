using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MySaver.Models;
using MySaver.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MySaver.ViewModels;

public partial class StoresViewModel : BaseViewModel
{
    IWebService webService;
    IAlert alert;
    IGeolocation geolocation;
    public ObservableCollection<Store> Stores { get; set; } = new ObservableCollection<Store>();
    public ObservableCollection<Store> MyItems { get; set; }
    public ICommand UpdateStoresCommand { get; }
    
    public StoresViewModel(IWebService webService, IAlert alert, IGeolocation geolocation)
    {
        this.webService = webService;
        this.alert = alert;
        this.geolocation = geolocation;
        UpdateStoresCommand = new Command(async () => await UpdateStoresAsync());
        MyItems = new ObservableCollection<Store>(Stores);
    }


    [ObservableProperty]
    bool isRefreshing;

    public async Task UpdateStoresAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            if (Stores.Count != 0)
            {
                Stores.Clear();
                MyItems.Clear();
            }

            var stores = await webService.GetObjectListAsync<Store>();

            if (stores != null)
            {
                foreach (var store in stores)
                {
                    Stores.Add(store);
                    MyItems.Add(store);
                }
                await GetDistance();
            }
        }
        catch (Exception ex)
        {
            await alert.DisplayAlert("Error!",
                $"Unable to get stores: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }

    }

    [ICommand]
    public async Task GetClosestStoreAsync()
    {
        if (IsBusy || Stores.Count == 0)
            return;

        try
        {
            var location = await geolocation.GetLastKnownLocationAsync();
            if (location is null)
            {
                location = await geolocation.GetLocationAsync(
                    new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30),

                    });
            }
            
            if (location is null)
                return;

            var list = Stores.OrderBy(s =>
                location.CalculateDistance(s.Latitude, s.Longitude, DistanceUnits.Kilometers)
                ).ToList();

            if (list is null)
                return;

            Stores.Clear();
            MyItems.Clear();
            foreach (var store in list)
            {
                Stores.Add(store);
                MyItems.Add(store);
            }
                
        }
        catch (Exception ex)
        {
            await alert.DisplayAlert("Error!",
                $"Unable to get closest store: {ex.Message}", "OK");
        }
    }

    public async Task GetDistance()
    {
        var location = await geolocation.GetLastKnownLocationAsync();

        if (location is null)
        {
            location = await geolocation.GetLocationAsync(
                new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30),
                });
        }

        if (location is null)
            return;

        foreach (var item in Stores)
        {
            double distance = location.CalculateDistance(item.Latitude, item.Longitude, DistanceUnits.Kilometers);

            item.Distance = Math.Round(distance, 1);
        }

    }
}
