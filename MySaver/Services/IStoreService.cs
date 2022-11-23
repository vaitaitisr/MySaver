using MySaver.Models;

namespace MySaver.Services;

public interface IStoreService
{
    public Task<List<Store>> GetStoresAsync();
}
