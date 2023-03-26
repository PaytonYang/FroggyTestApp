using CommunityToolkit.Mvvm.Messaging.Messages;
using FroggyTest.WPF.Models;

namespace FroggyTest.WPF.Messages;

public class UserLoginMessage : ValueChangedMessage<UserModel>
{
    public UserLoginMessage(UserModel value) : base(value) { }
}
