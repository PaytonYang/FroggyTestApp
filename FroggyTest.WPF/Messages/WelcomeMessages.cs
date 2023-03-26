using CommunityToolkit.Mvvm.Messaging.Messages;

namespace FroggyTest.WPF.Messages;

public class WelcomeCompletedMessage : ValueChangedMessage<bool>
{
    public WelcomeCompletedMessage(bool value) : base(value) { }
}
