using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using FroggyTest.WPF.Messages;
using FroggyTest.WPF.ViewModels;
using MaterialDesignThemes.Wpf;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace FroggyTest.WPF.Views;

public partial class MainView : Window
{
    public MainView(MainViewModel viewModel)
    {
        viewModel.UserLoginCallback = _onUserLoginCompleted;
        this.DataContext= viewModel;
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<WelcomeCompletedMessage>(this, (r, m) => _onWelcomeViewCompleted(m.Value));
        this.frame.Navigate(Ioc.Default.GetService<WelcomeView>());
        //this.frame.Navigate(Ioc.Default.GetService<CameraView>());
    }

    private void logoutButton_Click(object sender, RoutedEventArgs e)
    {
        this.frame.Navigate(Ioc.Default.GetService<UserLoginView>());
    }

    private void functionListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        this.frame.Navigate(this.functionListBox.SelectedItem);
    }

    private void _onWelcomeViewCompleted(bool isCompleted)
    {
        if (isCompleted) 
        {
            this.frame.Navigate(Ioc.Default.GetService<UserLoginView>());
        }
        else
        {
            this.Close();
        }
    }

    private void _onUserLoginCompleted()
    {
        this.frame.Navigate(Ioc.Default.GetService<BlankView>());
        this.functionListBox.Items.Clear();
        this.functionListBox.Items.Add(Ioc.Default.GetService<CameraView>());
        this.functionListBox.Items.Add(Ioc.Default.GetService<AboutView>());
    }

    private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        e.Cancel = true;
        bool close_confirmed = await ShowYesNoDialog.Send("Do you want to close this app?");
        if (close_confirmed) 
        {
            bool all_panel_closed = await AppClosingMessages.Send();
            if (all_panel_closed)
            {
                this.Closing -= Window_Closing;
                this.Close();
            }
        }
    }
}
