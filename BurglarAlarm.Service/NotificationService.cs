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

        public async Task<bool> CheckUploadImage(string serial)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var query = WarningListModel.ListModels.Where(w => w.Serial == serial).FirstOrDefault();

                    if (query != null)
                    {
                        if (query.StartDate >= DateTime.Now)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
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

        public async Task<bool> SendNotification(string serial, AppSetting appSetting)
        {
            var query = WarningListModel.ListModels.Where(w => w.Serial == serial).FirstOrDefault();

            if (query is not null)
            {
                query.StartDate = DateTime.Now.AddMinutes(10);

                return await notificationExternalService.SendNotification(serial, appSetting);
            }
            else
            {
                throw new Exception(Errors.Device_Not_Register);
            }
        }

        public async Task<string> UploadImage(string serial, string imageFile)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var query = WarningListModel.ListModels.Where(w => w.Serial == serial).FirstOrDefault();

                    if (query is null)
                    {
                        throw new Exception(Errors.Device_Not_Register);
                    }
                    if (query.StartDate >= DateTime.Now)
                    {
                        OnlineModel.Frame.Clear();
                        OnlineModel.Frame.Append(imageFile);

                        return Errors.Success;
                    }
                    else
                    {
                        throw new Exception(Errors.Upload_Image_Expire_DateTime);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
        }
    }
}
