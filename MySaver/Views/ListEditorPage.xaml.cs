using MySaver.Models;
using MySaver.Services;
using MySaver.ViewModels;

namespace MySaver.Views;

public partial class ListEditorPage : ContentPage
{
    private IProductListService listService;
    private string startName { get; set; } = "Titulas";
    private bool isBusy = false;
    private ListEditorViewModel viewModel;
    private string mainDir = FileSystem.Current.AppDataDirectory;

    public ListEditorPage(ListEditorViewModel viewModel, IProductListService listService)
    {
        BindingContext = viewModel;
        this.viewModel = viewModel;

        this.listService = listService;

        InitializeComponent();
    }

    async void OnTextChanged(object sender, EventArgs e)
    {
        SearchBar search = (SearchBar)sender;
        SearchResults.ItemsSource = await viewModel.GetSearchResultsAsync(search.Text);
    }

    void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        viewModel.AddProduct((Product)SearchResults.SelectedItem);
        Device.BeginInvokeOnMainThread(() => SearchResults.SelectedItem = null);
    }

    async void OnSaveTapped(object sender, EventArgs e)
    {
        listService.SaveList(viewModel.CurrentListName, viewModel.SelectedProducts);

        if (startName != viewModel.ListName)
        {
            if (listService.ListExists(viewModel.ListName))
            {
                bool answer = await DisplayAlert("Klausimas", "Ar norite perrašyti esantį failą?", "Taip", "Ne");
                if (!answer)
                {
                    return;
                }
            }

            listService.RenameList(viewModel.CurrentListName, viewModel.ListName);
            viewModel.CurrentListName = viewModel.ListName;

            startName = viewModel.ListName;
        }
    }

    protected override bool OnBackButtonPressed()
    {
        if (!isBusy)
        {
            var oldList = listService.OpenList(viewModel.CurrentListName);
            var newList = viewModel.SelectedProducts;
            //if filename was changed      or if the list contents were changed then renders popup
            if (viewModel.ListName != startName ||
                !(oldList?.SequenceEqual(newList) ?? !newList.Any()))
            {
                // TODO: show delete list pop-up if newList is empty
                PutSavePopup();
            }
            else
            {
                Shell.Current.GoToAsync("..");
            }
        }

        return true;
    }

    void OnRemoveProductTapped(object sender, EventArgs e)
    {
        var senderButton = (ImageButton)sender;
        viewModel.RemoveProduct((Product)senderButton.CommandParameter);
    }

    private async void PutSavePopup()
    {
        isBusy = true;
        {
            bool answer = await DisplayAlert("Klausimas", "Ar norite išsaugoti sąrašą?", "Taip", "Ne");
            if (answer)
            {
                OnSaveTapped(null, null);
            }
            await Shell.Current.GoToAsync("..");
            isBusy = false;
        }
    }

    void OnStepperValueChanged(object sender, ValueChangedEventArgs e)
    {
    }
}
