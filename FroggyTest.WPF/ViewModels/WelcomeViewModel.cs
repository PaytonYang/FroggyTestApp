using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Froggy.Database;
using FroggyTest.WPF.Messages;
using System;
using System.Threading.Tasks;

namespace FroggyTest.WPF.ViewModels;

public partial class WelcomeViewModel : ObservableObject
{
    private IDapperDB _database;

    [ObservableProperty]
    private int _progressStatus = 0;

    public WelcomeViewModel(IDapperDB database)
    {
        _database = database;
    }

    [RelayCommand]
    private async void WelcomeLoaded()
    {
        try
        {
            await Task.WhenAll(new[] { _database.CreateOrCheckDB(), _startupProcess() });
            WelcomeCompletedMessage.Send(true);
        }
        catch(Exception error)
        {
            ShowNormalDialog.Send($"Create database failed. Details: {error.GetBaseException().Message}");
            WelcomeCompletedMessage.Send(false);
        }
    }

    private async Task _startupProcess()
    {
        await Task.Run(() =>
        {
            for (int i = 1; i <= 4; i++)
            {
                this.ProgressStatus = i * 25;
                Task.Delay(1000).Wait();
            }
        });
    }
}
