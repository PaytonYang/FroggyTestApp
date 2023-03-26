using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FroggyTest.WPF.Messages;
using FroggyTest.WPF.Models;
using System;

namespace FroggyTest.WPF.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LogoutCommand))]
    private UserModel _user = new UserModel { UserName = "Guest" };

    [ObservableProperty]
    private bool _loggedIn = false;

    public Action? UserLoginCallback { get; set; } = null;

    public MainViewModel() 
    {
        WeakReferenceMessenger.Default.Register<UserLoginMessage>(this, (r, m) =>
        {
            this.LoggedIn = true;
            this.User = m.Value;
            UserLoginCallback?.Invoke();
        });
    }

    [RelayCommand(CanExecute =nameof(CanLogout))]
    private void Logout()
    {
        this.User = new UserModel { UserName = "Guest" };
        this.LoggedIn = false;
    }

    private bool CanLogout()
    {
        return this.LoggedIn;
    }
}
