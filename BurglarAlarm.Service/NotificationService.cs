using BurglarAlarm.Domain.Common;
using BurglarAlarm.Domain.Common.AppSettings;
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

        public async Task<bool> SendNotification(string serial, AppSetting appSetting)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var query = WarningListModel.ListModels.Where(w => w.Serial == serial).FirstOrDefault();

                    if (query != null)
                    {
                        query.StartDate = DateTime.Now.AddMinutes(10);

                        return await notificationExternalService.SendNotification(serial, appSetting);
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

        public async Task<string> UploadImage(string serial, string imageFile)
        {
            return await Task.Run(async() =>
            {
                try
                {
                    var query = WarningListModel.ListModels.Where(w => w.Serial == serial).FirstOrDefault();

                    if (query.StartDate >= DateTime.Now)
                    {
                        OnlineModel.Frame.Clear();
                        OnlineModel.Frame.Append(imageFile);

                        return "Success";
                    }
                    else
                    {
                        return "Faild";
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            });
        }
    }
}
