using BurglarAlarm.Domain.Common.AppSettings;
using System.Threading.Tasks;

namespace BurglarAlarm.ExternalService.Contract
{
    public interface INotificationExternalService
    {
        Task<bool> SendNotification(string serial, AppSetting appSetting);
    }
}
