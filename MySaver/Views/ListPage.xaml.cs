namespace MySaver.Views;

public partial class ListPage: ContentPage
{
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
	
	async void OnListTapped(object sender, ItemTappedEventArgs e)
    {
        await Navigation.PushAsync(new ListEditorPage(e.Item.ToString()));
        //await Navigation.PushAsync(new ListEditorPage("fftetstss"));
    }

    async void OnCreateTapped(object sender, EventArgs e)
	{
		string targetFile = Path.Combine(mainDir, "Untitled");
		
        using FileStream outputStream = File.OpenWrite(targetFile);
        using StreamWriter streamWriter = new StreamWriter(outputStream);

        await streamWriter.WriteAsync("the new one\n");
		await streamWriter.FlushAsync();

		RefreshLists();
        await Navigation.PushAsync(new ListEditorPage());
    }
}