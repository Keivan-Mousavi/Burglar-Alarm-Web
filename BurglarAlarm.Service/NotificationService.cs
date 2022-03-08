using BurglarAlarm.ExternalService.Contract;
using BurglarAlarm.Service.Contract;
using System;
using System.Threading.Tasks;

namespace BurglarAlarm.Service
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationExternalService notificationExternalService;

        public NotificationService(INotificationExternalService notificationExternalService)
        {
            this.notificationExternalService = notificationExternalService;
        }

        public async Task SendNotification()
        {
            await notificationExternalService.SendNotification();
        }
    }
}
