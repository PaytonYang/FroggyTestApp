using FroggyTest.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FroggyTest.WPF.Views;

public partial class UserLoginView : Page
{
    public UserLoginView(UserLoginViewModel viewModel)
    {
        viewModel.RegisterCompletedCallback = () => goToLogin_Click(this, new RoutedEventArgs());
        this.DataContext = viewModel;
        InitializeComponent();
        this.loginUserName.Text = "";
    }

    private void goToRegist_Click(object sender, RoutedEventArgs e)
    {
        this.registUserName.Text = "";
        this.email.Text = "";
        this.registPassword.Clear();
        this.confirmPassword.Clear();
        this.userTab.SelectedIndex = 1;
    }

    private void goToLogin_Click(object sender, RoutedEventArgs e)
    {
        this.loginUserName.Text = "";
        this.loginPassword.Clear();
        this.userTab.SelectedIndex = 0;
    }

    //Passwordbox doesn't have dependency property
    private void loginPassword_PasswordChanged(object sender, RoutedEventArgs e)
    {
        ((UserLoginViewModel)this.DataContext).LoginUser.Password = this.loginPassword.Password;
    }

    private void registPassword_PasswordChanged(object sender, RoutedEventArgs e)
    {
        ((UserLoginViewModel)this.DataContext).RegisterUser.Password = this.registPassword.Password;
    }

    private void confirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
    {
        ((UserLoginViewModel)this.DataContext).ConfirmPassword = this.confirmPassword.Password;
    }
}

//public class PasswordLengthValidationRule : ValidationRule
//{
//    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
//    {
//        if (value is PasswordBox)
//        { 
//            PasswordBox password_box = (PasswordBox)value;
//            return password_box.Password.Length >= 5 ? ValidationResult.ValidResult : new ValidationResult(false, "At least 5 characters");
//        }
//        else
//        {
//            return new ValidationResult(false, "Not Password Box");
//        }
//    }
//}
