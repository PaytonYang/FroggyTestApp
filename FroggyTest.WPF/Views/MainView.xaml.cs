using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using FroggyTest.WPF.Messages;
using FroggyTest.WPF.ViewModels;
using MaterialDesignThemes.Wpf;
using System;
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
}
