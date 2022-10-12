using MySaver.ViewModels;
using MySaver.Views;
using Syncfusion.Maui.Core.Hosting;

namespace MySaver;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<StoreService>();
		builder.Services.AddSingleton<StoresViewModel>();
        builder.Services.AddSingleton<StoresPage>();

		return builder.Build();
	}
}
