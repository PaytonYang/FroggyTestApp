using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Linq;
using System.Threading.Tasks;

namespace FroggyTest.WPF.Messages
{
    public class AppClosingMessages : CollectionRequestMessage<Task<bool>>
    {
        public static async Task<bool> Send()
        {
            var result_task = WeakReferenceMessenger.Default.Send(new AppClosingMessages()).Responses;
            if (result_task != null) 
            {
                var result = await Task.WhenAll(result_task);
                if (result != null) 
                {
                    return result.All(x => x == true);
                }
                else { return false; }
            }
            else 
            {
                return true;
            }
        }
    }
}
