using MySaver.Models;
using MySaver.Services;
using System.Text.Json;

namespace MySaver.Services;

public class StoreService : IStoreService
{
    private List<Store> storeList;
    public async Task<List<Store>> GetStoresAsync()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("stores.json");
        using var reader = new StreamReader(stream);
        var contents = await reader.ReadToEndAsync();
        storeList = JsonSerializer.Deserialize<List<Store>>(contents);

        return storeList;
    }
}
