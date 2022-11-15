using MySaver.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MySaver.ViewModels;

public class StoresViewModel
{
    StoreService storeService;
    public ObservableCollection<Store> Stores { get; } = new ObservableCollection<Store>();
    public ObservableCollection<Store> MyItems { get; set; }

    IGeolocation geolocation;
    
    public StoresViewModel(StoreService storeService, IGeolocation geolocation)
    {
        this.storeService = storeService;
        this.geolocation = geolocation;
        MyItems = new ObservableCollection<Store>(Stores);
    }

    public async Task UpdateStoresAsync()
    {
        if (Stores.Count != 0)
        {
            Stores.Clear();
            MyItems.Clear();
        }
            
        var stores = await storeService.GetStoresAsync();
        
        foreach (var store in stores)
        {
            Stores.Add(store);
            MyItems.Add(store);
        }
    }

    public async Task GetClosestStore()
    {
        try
        {
            var location = await geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                });
            }

            var closest = MyItems.OrderBy(m => location.CalculateDistance(
                new Location(m.Latitude, m.Longitude), DistanceUnits.Miles))
                .FirstOrDefault();

            var stores = await storeService.GetStoresAsync();
            if(closest != null)
            {
                MyItems.Clear();
                MyItems.Add(closest);
            }
            else
                await Shell.Current.DisplayAlert("No Stores Found", "", "OK");

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to query location: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
    }
 }
