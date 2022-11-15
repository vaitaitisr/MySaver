namespace MySaver.Views;
using Plugin.LocalNotification;

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

        var request = new NotificationRequest
        {
            NotificationId = 1,
            Title = "Changes were made",
            Description = "You've just changed your profile picture!",
            BadgeNumber = 42,
        };

        await LocalNotificationCenter.Current.Show(request);
    }
    async void OnThemeToolbarItemClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ThemeSelectionPage());
    }
}