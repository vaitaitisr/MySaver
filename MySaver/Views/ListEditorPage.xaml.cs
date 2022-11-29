using MySaver.Models;
using MySaver.Services;
using MySaver.ViewModels;

namespace MySaver.Views;

public partial class ListEditorPage : ContentPage, IQueryAttributable
{
    private IProductListService listService;
    private string startName { get; set; } = "Titulas";
    private bool isBusy = false;
    private ListEditorViewModel viewModel;
    private delegate void saveDel();

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

        // This refresh is useful since OnStepperValueChanged gets called only when the stepper
        // is being rendered (adding products doesn't refresh price total when off-screen)
        RefreshPriceTotal();

        Device.BeginInvokeOnMainThread(() => SearchResults.SelectedItem = null);
    }

    async void OnSaveTapped(object sender, EventArgs e)
    {
        saveDel saveAction = viewModel.SaveList;

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

            saveAction += viewModel.RenameList;

            startName = viewModel.ListName;
        }
        saveAction();
    }

    protected override bool OnBackButtonPressed()
    {
        if (!isBusy)
        {
            var oldList = viewModel.ReadList();
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
        RefreshPriceTotal();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("ListName"))
        {
            startName = query["ListName"] as string;
        }
    }

    public void RefreshPriceTotal()
    {
        TotalPrice.Text = viewModel.CalculateTotal().ToString("0.00") + "€";
    }
}
