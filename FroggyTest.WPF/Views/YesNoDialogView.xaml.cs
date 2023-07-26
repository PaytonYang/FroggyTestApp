using CommunityToolkit.Mvvm.Messaging;
using FroggyTest.WPF.Messages;
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

namespace FroggyTest.WPF.Views
{
    /// <summary>
    /// Interaction logic for YesNoDialogView.xaml
    /// </summary>
    public partial class YesNoDialogView : UserControl
    {
        public YesNoDialogView()
        {
            InitializeComponent();
            WeakReferenceMessenger.Default.Register<ShowYesNoDialog>(this, (sender, e) => 
            {
                this.message.Text = e.Message;
                e.Reply(DialogHost.Show(this, "RootDialog"));
            });
        }
    }
}
