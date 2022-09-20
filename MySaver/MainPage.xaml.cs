namespace MySaver;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{

	}
	private void OnGuestSignInClicked(object sender, EventArgs e)
	{
        Application.Current.MainPage.Navigation.PushModalAsync(new ProfilePage(), true);
    }
}

