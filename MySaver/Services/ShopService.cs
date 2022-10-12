using MySaver.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MySaver.Services
{
    public class ShopService
    {
        List<Controls.Shop> shopList = new();
        public async Task<List<Controls.Shop>> GetShops()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("storeInfo.json");
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            shopList = JsonSerializer.Deserialize<List<Controls.Shop>>(contents);

            return shopList;
        }
    }
}
