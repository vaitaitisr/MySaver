using MySaver.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MySaver.ViewModels;

public class StoresViewModel
{
    StoreService storeService;
    public ObservableCollection<Store> Stores { get; } = new ObservableCollection<Store>();
    public ICommand UpdateStoresCommand { get; }

    public StoresViewModel(StoreService storeService)
    {
        this.storeService = storeService;
        UpdateStoresCommand = new Command(async () => await UpdateStoresAsync());
    }

    public async Task UpdateStoresAsync()
    {
        if (Stores.Count != 0)
            Stores.Clear();

        var stores = await storeService.GetStoresAsync();

        foreach (var store in stores)
        {
            Stores.Add(store);
        }
    }

}
