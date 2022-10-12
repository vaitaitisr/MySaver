using MySaver.ShopView;

namespace MySaver.Views;

public partial class ShopPage : ContentPage
{
    public ShopPage(ShopsView shopsView)
    {
        InitializeComponent();
        BindingContext = shopsView;
    }
}