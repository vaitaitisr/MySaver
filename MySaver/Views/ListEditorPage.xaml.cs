﻿using MySaver.Models;
using MySaver.ViewModels;

namespace MySaver.Views;

public partial class ListEditorPage : ContentPage
{
    private string startName;
    private bool isBusy = false;
    private ProductViewModel viewModel;
    private string mainDir = FileSystem.Current.AppDataDirectory;
    
    public ListEditorPage(string inputName = "Titulas")
    {
        viewModel = new ProductViewModel(inputName);
        BindingContext = viewModel;

        InitializeComponent();

        startName = inputName;
    }

    protected override async void OnAppearing()
    {
        SearchResults.ItemsSource = await viewModel.GetSearchResultsAsync(null);
    }

    async void OnTextChanged(object sender, EventArgs e)
    {
        SearchBar search = (SearchBar)sender;
        SearchResults.ItemsSource = await viewModel.GetSearchResultsAsync(search.Text);
    }

    async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        GetProductDelegate addSelection = viewModel.AddSelection;
        addSelection((Product)SearchResults.SelectedItem);
        Device.BeginInvokeOnMainThread(() => SearchResults.SelectedItem = null);
    }

    async void OnSaveTapped(object sender, EventArgs e)
    {
        viewModel.SaveFile();
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

            viewModel.RenameFile(renamedFile);

            startName = viewModel.ListName;
        }
    }

    protected override bool OnBackButtonPressed()
    {
        if (!isBusy)
        {
            //if filename was changed      or if the list contents were changed then renders popup
            var check = viewModel.ReadList();
            if (viewModel.ListName != startName ||
                !viewModel.SelectedProducts.Equals(check))
            {
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
        GetProductDelegate removeProduct = viewModel.RemoveProduct;
        removeProduct((Product)senderButton.CommandParameter);
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
}
