using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging.Messages;
using FroggyTest.WPF.Views;

namespace FroggyTest.WPF.Messages;

public class ShowNormalDialogMessage : ValueChangedMessage<string>
{
    public ShowNormalDialogMessage(string message) : base(message) { var view = Ioc.Default.GetService<NormalDialogView>(); }
}
