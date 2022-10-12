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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.GetStoresCommand.Execute(null);
    }

}