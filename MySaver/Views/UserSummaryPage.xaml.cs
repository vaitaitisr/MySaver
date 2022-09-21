namespace MySaver.Views;

public partial class UserSummaryPage : ContentPage
{
	public UserSummaryPage()
	{
		InitializeComponent();
	}

	async void MoreInfo_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new UserDetailsPage());
	}
}