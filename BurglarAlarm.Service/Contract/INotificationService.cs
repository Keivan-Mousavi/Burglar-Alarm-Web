using System.Threading.Tasks;

namespace BurglarAlarm.Service.Contract
{
    public interface INotificationService
    {
        Task SendNotification();
    }
}
