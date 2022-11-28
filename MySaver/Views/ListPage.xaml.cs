using MySaver.Services;

namespace MySaver.Views;

public partial class ListPage : ContentPage
{
    private IProductListService listService;
    public ListPage(IProductListService listService)
    {
        this.listService = listService;

        InitializeComponent();
        RefreshLists();
    }

    protected override void OnAppearing()
    {
        RefreshLists();
        base.OnAppearing();
    }

    private void RefreshLists()
    {
        ListOfLists.ItemsSource = listService.GetListNames();
    }
    async void OnDeleteListTapped(object sender, EventArgs e)
    {
        var senderButton = (ImageButton)sender;
        await DeleteListAsync((string)senderButton.CommandParameter);
    }

    async Task DeleteListAsync(string listName)
    {
        if (listService.ListExists(listName))
        {
            bool answer = await DisplayAlert("Klausimas", "Ar norite ištrinti sąrašą?", "Taip", "Ne");
            if (!answer)
            {
                return;
            }
            listService.DeleteList(listName);
            RefreshLists();
        }
    }

    async void OnListTapped(object sender, SelectionChangedEventArgs e)
    {
        if (ListOfLists.SelectedItem != null)
        {
            await Shell.Current.GoToAsync("ListEditor", new Dictionary<string, object> 
            {
                { "ListName", e.CurrentSelection.FirstOrDefault() } 
            });
            
            ListOfLists.SelectedItem = null;
        }
    }

    async void OnCreateTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("ListEditor");
    }
}
