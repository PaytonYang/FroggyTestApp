using CommunityToolkit.Mvvm.Messaging;
using FroggyTest.WPF.Messages;
using FroggyTest.WPF.ViewModels;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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

public partial class NormalDialogView : UserControl
{
    public NormalDialogView(NormalDialogViewModel viewModel)
    {
        this.DataContext = viewModel;
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<ShowNormalDialogMessage>(this, (r, m) => 
        {
            string message = m.Value;
            this.message.Text = message;
            DialogHost.Show(this, "RootDialog");
        });
    }
}
