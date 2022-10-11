using MySaver.Controls;

namespace MySaver.Views;

public partial class ListEditorPage : ContentPage
{
    DataClass dataManager = new DataClass();
    private string startName;
    private string mainDir, targetFile;
    public ListEditorPage(string inputName = "Untitled")
	{
        mainDir = FileSystem.Current.AppDataDirectory;
        targetFile = Path.Combine(mainDir, inputName);

        InitializeComponent();
        listName.Text = inputName;
        startName = inputName;
    }
    async void OnTextChanged(object sender, EventArgs e)
    {
        SearchBar search = (SearchBar)sender;
        SearchResults.ItemsSource = await dataManager.GetSearchResultsAsync(search.Text);
    }
    async void OnItemTapped(object sender, EventArgs e)
    {
        return;
    }
    async void OnSaveTapped(object sender, EventArgs e)
    {
        using FileStream outputStream = File.OpenWrite(targetFile);
        using StreamWriter streamWriter = new StreamWriter(outputStream);

        if (startName != listName.Text)
        {
            var renamedFile = Path.Combine(mainDir, listName.Text);

            streamWriter.Close();
            File.Delete(renamedFile);
            File.Move(targetFile, renamedFile);

            targetFile = renamedFile;
            startName = listName.Text;
        }
    }
}