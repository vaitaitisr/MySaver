namespace MySaver.Views;

public partial class ListPage : ContentPage
{
    string mainDir;
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

    async void OnListTapped(object sender, ItemTappedEventArgs e)
    {
        await Navigation.PushAsync(new ListEditorPage(inputName: e.Item.ToString()));
    }

    async void OnCreateTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ListEditorPage());
    }
}