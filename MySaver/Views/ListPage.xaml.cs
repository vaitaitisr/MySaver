namespace MySaver.Views;

public partial class ListPage : ContentPage
{
    private string mainDir;
    public ListPage()
    {
        mainDir = FileSystem.Current.AppDataDirectory;

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
        var tempList = Directory.GetFiles(mainDir);

        var fileNames =
            from file in tempList
            select Path.GetFileNameWithoutExtension(file);

        ListOfLists.ItemsSource = fileNames;
    }
    async void OnDeleteListTapped(object sender, EventArgs e)
    {
        var senderButton = (ImageButton)sender;
        DeleteListAsync((string)senderButton.CommandParameter);
    }

    async void DeleteListAsync(string listName)
    {
        var target = Path.Combine(mainDir, listName + ".json");
        if (File.Exists(target))
        {
            bool answer = await DisplayAlert("Klausimas", "Ar norite ištrinti sąrašą?", "Taip", "Ne");
            if (!answer)
            {
                return;
            }
            File.Delete(target);
            RefreshLists();
        }
    }

    async void OnListTapped(object sender, SelectionChangedEventArgs e)
    {
        if (ListOfLists.SelectedItem != null)
        {
            await Navigation.PushAsync(new ListEditorPage(inputName: e.CurrentSelection.FirstOrDefault().ToString()));
            ListOfLists.SelectedItem = null;
        }
    }

    async void OnCreateTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ListEditorPage());
    }
}
