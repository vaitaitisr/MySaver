using MySaver.ViewModels;

namespace MySaver.Views;

public partial class StoresPage : ContentPage
{
    private StoresViewModel viewModel;
    public StoresPage(StoresViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        this.viewModel = viewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.UpdateStoresAsync();
    }

}