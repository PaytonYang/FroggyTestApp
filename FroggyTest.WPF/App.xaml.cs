using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection;
using FroggyTest.WPF.Views;
using FroggyTest.WPF.ViewModels;
using Froggy.Database;

namespace FroggyTest.WPF;

public partial class App : Application
{
    public App()
    {
        //Create Service Collection
        ServiceCollection services = new ServiceCollection();

        //Add MainView, MainViewModel to Service
        services.AddSingleton<MainView>();
        services.AddSingleton<MainViewModel>();

        //Add WelcomeView, WelcomeViewModel to Service
        services.AddSingleton<WelcomeView>();
        services.AddSingleton<WelcomeViewModel>();

        //Add UserLoginView, UserLoginViewModel to Service
        services.AddSingleton<UserLoginView>();
        services.AddSingleton<UserLoginViewModel>();

        //Add CameraView, CameraViewModel to Service
        services.AddSingleton<CameraView>();
        services.AddSingleton<CameraViewModel>();

        //Add AboutView to Service
        services.AddSingleton<AboutView>();
        
        //Add BlankView to Service
        services.AddSingleton<BlankView>();

        //Add MessageDialogView, MessageDialogViewModel to Service
        services.AddTransient<NormalDialogView>();
        services.AddTransient<NormalDialogViewModel>();

        //Add Database Service
        services.AddSingleton<IDapperDB>(s => new SqliteDB("froggy.sqlite", @"Models\DBSchema\CreateDB.sql"));

        //Configre services to Ioc
        Ioc.Default.ConfigureServices(services.BuildServiceProvider());
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        //Launch MainView and ViewModel
        var main_view = Ioc.Default.GetRequiredService<MainView>();
        main_view.Show();
    }
}
