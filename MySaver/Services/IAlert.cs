namespace MySaver.Services
{
    public interface IAlert
    {
        Task DisplayAlert(string title, string message, string cancel);
        Task DisplayAlert(string title, string message, string accept, string cancel);
    }
}