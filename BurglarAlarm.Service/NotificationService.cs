using BurglarAlarm.Domain.Common;
using BurglarAlarm.ExternalService.Contract;
using BurglarAlarm.Service.Contract;
using System;
using System.Linq;
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

        public async Task<bool> SendNotification(string serial)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var query = WarningListModel.ListModels.Where(w => w.Serial == serial).FirstOrDefault();

                    if (query != null)
                    {
                        query.StartDate = DateTime.Now.AddMinutes(10);

                        return await notificationExternalService.SendNotification(serial);
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            });
        }
    }
}
