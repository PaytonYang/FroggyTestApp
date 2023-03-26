using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using FroggyTest.WPF.Messages;
using FroggyTest.WPF.Views;
using MaterialDesignThemes.Wpf;

namespace FroggyTest.WPF.ViewModels
{
    public partial class NormalDialogViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _message = "";
    }
}
