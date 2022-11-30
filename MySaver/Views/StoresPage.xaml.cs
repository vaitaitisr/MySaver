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
        await viewModel.GetClosestStoreAsync();
    }
    
    public async void ShopsFilterByName(object sender, TextChangedEventArgs e)
    {
       
        var searchTerm = e.NewTextValue;

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = string.Empty;
        }

        searchTerm = searchTerm.ToLowerInvariant();

        var filteredItems = viewModel.Stores.Where(
            Store => Store.Name.ToLowerInvariant().Contains(searchTerm)).ToList();


        viewModel.MyItems.Clear();

        foreach (var Item in filteredItems)
        {
            viewModel.MyItems.Add(Item); 
        }

        await viewModel.GetClosestStoreAsync();
    }
}