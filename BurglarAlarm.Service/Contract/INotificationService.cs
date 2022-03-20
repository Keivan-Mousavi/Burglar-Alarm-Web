using BurglarAlarm.Domain.Common.AppSettings;
using System.Threading.Tasks;

namespace BurglarAlarm.Service.Contract
{
    public interface INotificationService
    {
        Task<bool> SendNotification(string serial, AppSetting appSetting);

        Task<string> UploadImage(string serial, string imageFile);

        Task<bool> CheckUploadImage(string serial);

        Task<string> ControllerTV(string serial);

        Task<bool> AddControllerTV(string serial, string sendNEC);
    }
}
