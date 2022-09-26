using MySaver.Controls;
using System.Collections.ObjectModel;

namespace MySaver.Views;

public partial class ListPage: ContentPage
{
	DataClass dataManager = new DataClass();

	public ListPage()
	{
		InitializeComponent();
	}

	async void OnTextChanged(object sender, EventArgs e) 
	{
		SearchBar search = (SearchBar)sender;
		SearchResults.ItemsSource = await dataManager.GetSearchResultsAsync(search.Text);
    }
}