using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using FroggyTest.WPF.Models;

namespace FroggyTest.WPF.Messages;

public class UserLoginMessage : ValueChangedMessage<UserModel>
{
    public UserLoginMessage(UserModel value) : base(value) { }

    public static void Send(UserModel currentUser)
    {
        WeakReferenceMessenger.Default.Send(new UserLoginMessage(currentUser));
    }
}
