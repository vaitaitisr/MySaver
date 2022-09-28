namespace MySaver;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
	}

	private async void OnSignOutClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//LoginPage");
	}
}
