﻿using MySaver.Services;
using MySaver.ViewModels;
using MySaver.Views;
using Syncfusion.Maui.Core.Hosting;
using Plugin.LocalNotification;

namespace MySaver;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseLocalNotification()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
        builder.Services.AddSingleton<IMap>(Map.Default);

        builder.Services.AddSingleton<StoreService>();
        builder.Services.AddSingleton<StoresViewModel>();
        builder.Services.AddSingleton<StoresPage>();

        builder.Services.AddSingleton<ProductService>();
        builder.Services.AddSingleton<ProductViewModel>();
        builder.Services.AddSingleton<ListEditorPage>();

        

        return builder.Build();
    }
}
