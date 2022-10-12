namespace MySaver.Views;

public partial class ListEditorPage : ContentPage
{
    DataClass dataManager = new DataClass();
    private string startName;
    private string mainDir, targetFile;
    private List<string> selection = new List<string>();

    public ListEditorPage(string inputName = "Untitled")
    {
        mainDir = FileSystem.Current.AppDataDirectory;
        targetFile = Path.Combine(mainDir, inputName);

        if (File.Exists(targetFile))
        {
            selection.AddRange(File.ReadAllLines(targetFile));
        }

        InitializeComponent();
        RefreshList();

        listName.Text = inputName;
        startName = inputName;
    }

    async void OnTextChanged(object sender, EventArgs e)
    {
        SearchBar search = (SearchBar)sender;
        SearchResults.ItemsSource = await dataManager.GetSearchResultsAsync(search.Text);
    }

    async void OnItemTapped(object sender, ItemTappedEventArgs e)
    {
        selection.Add(e.Item.ToString().TrimEnd());
        RefreshList();
    }

    private void RefreshList()
    {
        listContents.ItemsSource = selection.ToArray();
    }

    private void OnSaveTapped(object sender, EventArgs e)
    {
        File.WriteAllLines(targetFile, selection);

        if (startName != listName.Text)
        {
            var renamedFile = Path.Combine(mainDir, listName.Text);

            File.Delete(renamedFile);
            File.Move(targetFile, renamedFile);

            targetFile = renamedFile;
            startName = listName.Text;
        }
    }
}