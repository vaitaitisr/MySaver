using MySaver.Models;
using MySaver.ViewModels;

namespace MySaver.Views;

public partial class ListEditorPage : ContentPage, IQueryAttributable
{
    private string startName { get; set; } = "Titulas";
    private bool isBusy = false;
    private ProductViewModel viewModel;
    private string mainDir = FileSystem.Current.AppDataDirectory;
    private delegate void saveDel();

    public ListEditorPage(ProductViewModel viewModel)
    {
        BindingContext = viewModel;
        this.viewModel = viewModel;

        InitializeComponent();
    }

    async void OnTextChanged(object sender, EventArgs e)
    {
        SearchBar search = (SearchBar)sender;
        SearchResults.ItemsSource = await viewModel.GetSearchResultsAsync(search.Text);
    }

    async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        viewModel.AddSelection((Product)SearchResults.SelectedItem);
        Device.BeginInvokeOnMainThread(() => SearchResults.SelectedItem = null);
    }

    async void OnSaveTapped(object sender, EventArgs e)
    {
        saveDel saveAction = viewModel.SaveFile;

        var renamedFile = Path.Combine(mainDir, viewModel.ListName + ".json");

        if (startName != viewModel.ListName)
        {
            if (File.Exists(renamedFile))
            {
                bool answer = await DisplayAlert("Klausimas", "Ar norite perrašyti esantį failą?", "Taip", "Ne");
                if (!answer)
                {
                    return;
                }
            }

            saveAction += viewModel.RenameFile;

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

    async void OnRemoveProductTapped(object sender, EventArgs e)
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

    async void OnStepperValueChanged(object sender, ValueChangedEventArgs e)
    {
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("ListName"))
        {
            startName = query["ListName"] as string;
        }
    }
}
