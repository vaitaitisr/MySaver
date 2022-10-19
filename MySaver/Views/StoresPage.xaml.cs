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
    
    public void ShopsFilterByName(object sender, TextChangedEventArgs e)
    {
       
        var searchTerm = e.NewTextValue;

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = string.Empty;
        }

        searchTerm = searchTerm.ToLowerInvariant();

        var filteredItems = viewModel.Stores.Where(Store => Store.Name.ToLowerInvariant().Contains(searchTerm)).ToList();


        foreach(var Store in viewModel.Stores)
        {
            if (!filteredItems.Contains(Store))
            {
                viewModel.MyItems.Remove(Store);
            }
            else if (!viewModel.MyItems.Contains(Store))
            {
                viewModel.MyItems.Add(Store);
            }
        }
    }
}