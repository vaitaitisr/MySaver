using System.Text.RegularExpressions;

namespace MySaver.Views;

public partial class LoginPage : ContentPage
{

	public LoginPage()
	{
		InitializeComponent();
	}

	private async void OnSignInButtonClicked(object sender, EventArgs e)
	{
		var email = EmailEntry.Text;

		var emailPattern = "^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";


        if (Regex.IsMatch(email, emailPattern))
			await Shell.Current.GoToAsync("//AboutPage");
		else
			ErrorLabel.Text = "Invalid email";

	}
}

