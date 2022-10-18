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

    private async void AddUserImage_Clicked(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            FileTypes = FilePickerFileType.Images
        });

        if (result == null)
            return;

        var stream = await result.OpenReadAsync();

        profileImage.Source = ImageSource.FromStream(() => stream);
    }
    async void OnThemeToolbarItemClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ThemeSelectionPage());
    }
}