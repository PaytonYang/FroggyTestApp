using FroggyTest.WPF.ViewModels;
using System.Windows.Controls;

namespace FroggyTest.WPF.Views;

public partial class WelcomeView : Page
{
    public WelcomeView(WelcomeViewModel viewModel)
    {
        this.DataContext = viewModel;
        InitializeComponent();
    }


}
