using System.Text.RegularExpressions;
using MySaver.Controls;
using System.Collections.ObjectModel;

namespace MySaver.Views;

public partial class ListPage: ContentPage
{
	DataClass dataManager = new DataClass();
	string mainDir;
	public ListPage()
	{
		mainDir = FileSystem.Current.AppDataDirectory;

		InitializeComponent();
		RefreshLists();
    }
	private void RefreshLists()
	{
        var tempList = Directory.GetFiles(mainDir);

        var fileNames =
			from file in tempList
			select file.Replace(mainDir + "\\", "");

        ListOfLists.ItemsSource = fileNames;
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
	async void OnListTapped(object sender, EventArgs e)
    {
        return;
    }

    async void OnCreateTapped(object sender, EventArgs e)
	{
		string targetFile = System.IO.Path.Combine(mainDir, "Untitled");
			

        using FileStream outputStream = System.IO.File.OpenWrite(targetFile);
        using StreamWriter streamWriter = new StreamWriter(outputStream);

        await streamWriter.WriteAsync("the new one\n");
		await streamWriter.FlushAsync();

		RefreshLists();
    }
}