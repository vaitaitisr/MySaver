using CommunityToolkit.Mvvm.Input;
using MySaver.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace MySaver.ViewModels;

public partial class StoresViewModel : BaseViewModel
{
    StoreService storeService;
    public ObservableCollection<Store> Stores { get; } = new ObservableCollection<Store>();
    public ObservableCollection<Store> MyItems { get; set; }
    public ICommand UpdateStoresCommand { get; }

    IGeolocation geolocation;
    
    public StoresViewModel(StoreService storeService, IGeolocation geolocation)
    {
        this.storeService = storeService;
        this.geolocation = geolocation;
        UpdateStoresCommand = new Command(async () => await UpdateStoresAsync());
        MyItems = new ObservableCollection<Store>(Stores);
    }

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
            
            var stores = await storeService.GetStoresAsync();
        
            foreach (var store in stores)
            {
                Stores.Add(store);
                MyItems.Add(store);
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!",
                $"Unable to get stores: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }

    }

    [ICommand]
    async Task GetClosestStoreAsync()
    {
        if(IsBusy || Stores.Count == 0)
            return;

        try
        {
            var location = await geolocation.GetLastKnownLocationAsync();
            if(location is null)
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

            var first = Stores.OrderBy(s =>
                location.CalculateDistance(s.Latitude, s.Longitude, DistanceUnits.Kilometers)
                ).FirstOrDefault();

            if (first is null)
                return;

            await Shell.Current.DisplayAlert("Closest store",
                $"{first.Name} in {first.Address}", "OK");
        }
        catch(Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!",
                $"Unable to get closest store: {ex.Message}", "OK");
        }
    }
 }
