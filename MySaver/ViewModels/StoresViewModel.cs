using MySaver.Models;
using MySaver.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MySaver.ViewModels;

public class StoresViewModel
{
    IStoreService storeService;
    public ObservableCollection<Store> Stores { get; } = new ObservableCollection<Store>();
    public ObservableCollection<Store> MyItems { get; set; }
    public ICommand UpdateStoresCommand { get; }
    
    public StoresViewModel(IStoreService storeService)
    {
        this.storeService = storeService;
        UpdateStoresCommand = new Command(async () => await UpdateStoresAsync());
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
 }
