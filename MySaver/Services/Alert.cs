﻿namespace MySaver.Services;

public class Alert : IAlert
{
    public async Task DisplayAlert(string title, string message, string cancel)
    {
        await Shell.Current.DisplayAlert(title, message, cancel);
    }

    public async Task DisplayAlert(string title, string message, string accept, string cancel)
    {
        await Shell.Current.DisplayAlert(title, message, accept, cancel);
    }
}
