using MySaver.Controls;

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
        else
        {
            File.Create(targetFile).Close();
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

    async void OnSaveTapped(object sender, EventArgs e)
    {
        SaveFile();
    }

    private async void SaveFile()
    {
        File.WriteAllLines(targetFile, selection);

        if (startName != listName.Text)
        {
            var renamedFile = Path.Combine(mainDir, listName.Text);

            if (File.Exists(renamedFile))
            {
                bool answer = await DisplayAlert("Klausimas", "Ar norite perrašyti esantį failą?", "Taip", "Ne");
                if (!answer)
                {
                    return;
                }
            }

            File.Delete(renamedFile);
            File.Move(targetFile, renamedFile);

            targetFile = renamedFile;
            startName = listName.Text;
        }
    }

    protected override async void OnDisappearing()
    {
        //if filename was changed      or if the list contents were changed
        if (listName.Text != startName || !selection.SequenceEqual(File.ReadAllLines(targetFile)))
        {
            bool answer = await DisplayAlert("Klausimas", "Ar norite išsaugoti sąrašą?", "Taip", "Ne");

            if (answer)
            {
                SaveFile();
            }
        }
    }
}
