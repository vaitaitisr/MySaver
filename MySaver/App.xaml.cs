using Microsoft.Maui.Platform;
using MySaver.Handlers;
using MySaver.Services;

namespace MySaver;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		//Borderless entry
		Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
		{
			if(view is BorderlessEntry)
			{
#if ANDROID
				handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
#endif
			}
		});

        MauiExceptions.UnhandledException += (sender, args) =>
        {
            var exception = (Exception)args.ExceptionObject;
            LogService.Log(exception.ToString());
        };

		MainPage = new AppShell();
	}
}
