using MySaver.Services;
using MySaver.Views;
using MySaver.ShopView;

namespace MySaver;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddTransient<ShopPage>();
        builder.Services.AddSingleton<ShopService>();
        builder.Services.AddTransient<ShopsView>();

		return builder.Build();
	}
}
