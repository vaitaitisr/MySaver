namespace MySaver.Views;

public partial class LoginPage : ContentPage
{

	public LoginPage()
	{
		InitializeComponent();
	}

	private async void OnSignInButtonClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//AboutPage");
	}
}

