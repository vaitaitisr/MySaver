namespace MySaver;

public partial class ListPage: ContentPage
{
	private DataClass dataClass = new DataClass();
	public ListPage()
	{
		InitializeComponent();

	}
	async void OnTextChanged(object sender, EventArgs e) 
	{
		SearchBar search = (SearchBar)sender;
		SearchResults.ItemsSource = await dataClass.GetSearchResultsAsync(search.Text);
    }
}