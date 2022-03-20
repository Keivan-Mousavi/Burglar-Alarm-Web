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

        public async Task<bool> AddControllerTV(string serial, string sendNEC)
        {
            return await Task.Run(() =>
            {
                try
                {
                    ListControllerModel.ListController.Add(new ControllerModel
                    {
                        SendNEC = sendNEC,
                        Serial = serial
                    });

                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
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

        public async Task<string> ControllerTV(string serial)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var query = WarningListModel.ListModels.Where(w => w.Serial == serial).FirstOrDefault();

                    var dt = DateTime.Now;

                    if (query is null)
                    {
                        return Errors.Not_Find_This_Device;
                    }
                    if (query.Serial == serial && query.StartDate >= dt)
                    {
                        var lcm = ListControllerModel.ListController.FirstOrDefault(f => f.Serial == serial);
                        if (lcm is null)
                        {
                            return Errors.Not_Find_List_Controller;
                        }

                        ListControllerModel.ListController.Remove(lcm);

                        return lcm.SendNEC;
                    }
                    else
                    {
                        return Errors.Not_Find_This_Device;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
        }

        public async Task<string> ListAllControllerTV(string serial)
        {
            return await Task.Run(() =>
            {
                var query = ListControllerModel.ListController.Where(w => w.Serial == serial).OrderBy(o => o.Id).FirstOrDefault();

                ListControllerModel.ListController.Remove(query);

                if(query is null)
                {
                    return string.Empty;
                }

                return query.SendNEC;
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
