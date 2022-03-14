using BurglarAlarm.Domain.Common;
using BurglarAlarm.Domain.Common.AppSettings;
using BurglarAlarm.Service.Component;
using BurglarAlarm.Service.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BurglarAlarm.Web.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ManageNotificationController : ControllerBase
    {
        private readonly INotificationService notificationService;

        private readonly IConfiguration configuration;

        public ManageNotificationController(INotificationService notificationService,
                                            IConfiguration configuration)
        {
            this.notificationService = notificationService;
            this.configuration = configuration;
        }

        [HttpPost(Name = "UploadImage")]
        public async Task<string> UploadImage(IFormFile imageFile)
        {
            string serial = HttpContext.Request.Headers["Serial"];

            return await notificationService.UploadImage(serial, imageFile.OpenReadStream().ConvertToBase64());
        }

        [HttpGet(Name = "ShowFrame")]
        public async Task<string> ShowFrame()
        {
            return await Task.Run(() =>
            {
                if (OnlineModel.Frame != null)
                {
                    return OnlineModel.Frame.ToString();
                }
                else
                {
                    return default;
                }
            });
        }

        [HttpGet(Name = "CheckCamera")]
        public async Task<bool> CheckCamera(string serial)
        {
            var appSetting = configuration.Get<AppSetting>();

            return await notificationService.SendNotification(serial, appSetting);
        }

        [HttpGet(Name = "CheckUploadImage")]
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

        [HttpGet(Name = "ControllerTV")]
        public async Task<string> ControllerTV(string serial)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var query = WarningListModel.ListModels.Where(w => w.Serial == serial).FirstOrDefault();

                    var dt = DateTime.Now;

                    if (query.Serial == serial && query.StartDate >= dt)
                    {
                        var lcm = ListControllerModel.ListController.FirstOrDefault(f => f.Serial == serial);
                        ListControllerModel.ListController.Remove(lcm);

                        return lcm.SendNEC;
                    }

                    return "not_find_this_device";
                }
                catch (Exception ex)
                {
                    return "error";
                }
            });
        }

        [HttpGet(Name = "AddControllerTV")]
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
                    return false;
                }
            });
        }

        [HttpGet(Name = "ListAllControllerTV")]
        public async Task<string> ListAllControllerTV(string serial)
        {
            return await Task.Run(() =>
            {
                var query = ListControllerModel.ListController.Where(w => w.Serial == serial).OrderBy(o => o.Id).FirstOrDefault();

                ListControllerModel.ListController.Remove(query);

                return query?.SendNEC;
            });
        }

        [HttpGet(Name = "AddDeviceIdNotification")]
        public async Task<bool> AddDeviceIdNotification(string key)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (!ListDeviceIdNotification.ListDeviceId.Any(a => a == key))
                    {
                        ListDeviceIdNotification.ListDeviceId.Add(key);
                    }

                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }
    }
}