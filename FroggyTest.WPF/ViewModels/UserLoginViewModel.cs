using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Froggy.Database;
using FroggyTest.WPF.Models;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using FroggyTest.WPF.Messages;
using System;

namespace FroggyTest.WPF.ViewModels;

public partial class UserLoginViewModel : ObservableObject
{
    private IDapperDB _database;

    [ObservableProperty]
    private UserModel _loginUser = new UserModel();

    [ObservableProperty]
    private UserModel _registerUser = new UserModel();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string _confirmPassword = "";

    public Action? RegisterCompletedCallback { get; set; } = null;

    public UserLoginViewModel(IDapperDB database)
    {
        _database = database;
        this.LoginUser.PropertyChanged += (sender, e) => this.LoginCommand.NotifyCanExecuteChanged();
        this.RegisterUser.PropertyChanged += (sender, e) => this.RegisterCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    private void LoginLoaded()
    {
        this.LoginUser.RevalidateProperty();
        this.RegisterUser.RevalidateProperty();
    }

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task Login()
    {
        try
        {
            UserModel? current_user = await UserModel.CheckUserExitsted(_database, this.LoginUser);
            if (current_user == null)
            {
                ShowNormalDialog.Send("User name is not existed or password is incorrected.");
            }
            else
            {
                UserLoginMessage.Send(current_user);
            }
        }
        catch { throw; }
    }

    private bool CanLogin()
    {
        var errors = this.LoginUser.GetErrors();
        return this.LoginUser.GetErrors().Where(x => x.MemberNames.Any(y => y == "UserName" | y == "Password")).Count() == 0;
    }

    [RelayCommand(CanExecute = nameof(CanRegister))]
    private async Task Register()
    {
        try
        {
            UserModel? current_user = await UserModel.CheckUserExitsted(_database, this.RegisterUser);
            if (current_user != null) { ShowNormalDialog.Send($"User name {this.RegisterUser.UserName} is existed."); return; }
            if(this.RegisterUser.Password != this.ConfirmPassword) { ShowNormalDialog.Send($"Confirm password is not correct."); return; }
            await UserModel.AddUser(_database, this.RegisterUser);
            RegisterCompletedCallback?.Invoke();
        }
        catch { throw; }
    }

    private bool CanRegister()
    {
        return !this.RegisterUser.HasErrors && !string.IsNullOrEmpty(this.ConfirmPassword);
    }
}
