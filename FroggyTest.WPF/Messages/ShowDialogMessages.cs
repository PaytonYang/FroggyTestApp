using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using FroggyTest.WPF.Views;
using System.Threading.Tasks;

namespace FroggyTest.WPF.Messages;

public class ShowNormalDialog : ValueChangedMessage<string>
{
    public ShowNormalDialog(string message) : base(message) { var view = Ioc.Default.GetService<NormalDialogView>(); }

    public static void Send(string message)
    {
        WeakReferenceMessenger.Default.Send(new ShowNormalDialog(message));
    }
}

public class ShowYesNoDialog : RequestMessage<Task<object?>>
{
    public string Message { get; set; }
    public ShowYesNoDialog(string message) 
    { 
        Message = message;
        Ioc.Default.GetService<YesNoDialogView>(); 
    }

    public static async Task<bool> Send(string message)
    {
        var result_task = WeakReferenceMessenger.Default.Send(new ShowYesNoDialog(message)).Response;
        if(result_task != null)
        {
            object? result = await result_task;
            return result != null && (bool)result;
        }
        else
        {
            return false;
        }

    }
}
