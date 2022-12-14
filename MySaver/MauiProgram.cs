using MySaver.Services;
using MySaver.ViewModels;
using MySaver.Views;
using Syncfusion.Maui.Core.Hosting;
using Plugin.LocalNotification;
using System.Diagnostics.CodeAnalysis;

namespace MySaver;

[ExcludeFromCodeCoverage]
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
#if !NET6_0
            .UseLocalNotification()
#endif
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
        builder.Services.AddSingleton<IMap>(Map.Default);

        builder.Services.AddSingleton<IAlert, Alert>();
        builder.Services.AddSingleton<IWebService, WebService>();
        builder.Services.AddSingleton<IProductListService, ProductListService>();
        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddSingleton<StoresViewModel>();
        builder.Services.AddSingleton<StoresPage>();
        builder.Services.AddSingleton<ListPage>();

        builder.Services.AddTransient<ListEditorViewModel>();
        builder.Services.AddTransient<ListEditorPage>();

        return builder.Build();
    }
}
