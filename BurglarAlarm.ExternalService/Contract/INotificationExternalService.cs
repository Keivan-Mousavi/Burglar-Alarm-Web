using System.Threading.Tasks;

namespace BurglarAlarm.ExternalService.Contract
{
    public interface INotificationExternalService
    {
        Task<bool> SendNotification(string serial);
    }
}
