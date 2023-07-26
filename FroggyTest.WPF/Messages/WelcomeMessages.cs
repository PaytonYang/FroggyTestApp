using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace FroggyTest.WPF.Messages;

public class WelcomeCompletedMessage : ValueChangedMessage<bool>
{
    public WelcomeCompletedMessage(bool value) : base(value) { }

    public static void Send(bool value)
    {
        WeakReferenceMessenger.Default.Send(new WelcomeCompletedMessage(value));
    }
}
