using MySaver.Controls;
using MySaver.Services;
using System.Collections.ObjectModel;

namespace MySaver.ShopView
{
    public partial class ShopsView : BaseView
    {
        public ObservableCollection<Shop> Shops { get; } = new();
        public Command GetShopsCommand { get; }
        ShopService shopService;
        public ShopsView(ShopService shopService)
        {
            this.shopService = shopService;
            GetShopsCommand = new Command(async () => await GetShopsAsync());
        }

        async Task GetShopsAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var shops = await shopService.GetShops();

                if (Shops.Count != 0)
                    Shops.Clear();

                foreach (var shop in shops)
                    Shops.Add(shop);

            }
            catch (Exception ex)
            {
               
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
