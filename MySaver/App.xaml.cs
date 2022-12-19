using Microsoft.Maui.Platform;
using MySaver.Handlers;
using MySaver.Services;

namespace MySaver;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        MauiExceptions.UnhandledException += (sender, args) =>
        {
            var exception = (Exception)args.ExceptionObject;
            LogService.Log(exception.ToString());
        };

		MainPage = new AppShell();
	}
}
