using BurglarAlarm.Domain.Common.AppSettings;
using System.Threading.Tasks;

namespace BurglarAlarm.Service.Contract
{
    public interface INotificationService
    {
        Task<bool> SendNotification(string serial, AppSetting appSetting);
    }
}
