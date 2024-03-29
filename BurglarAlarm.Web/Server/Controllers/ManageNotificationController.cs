﻿using BurglarAlarm.Domain.Common;
using BurglarAlarm.Domain.Common.AppSettings;
using BurglarAlarm.Service.Component;
using BurglarAlarm.Service.Contract;
using BurglarAlarm.Web.Server.Component;
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

        [HttpGet(Name = "CheckCamera")]
        [IgnoreMiddleware(IgnoreMiddleware.Never)]
        public async Task<bool> CheckCamera(string serial)
        {
            var appSetting = configuration.Get<AppSetting>();

            return await notificationService.SendNotification(serial, appSetting);
        }

        [HttpGet(Name = "CheckUploadImage")]
        public async Task<bool> CheckUploadImage(string serial) =>
             await notificationService.CheckUploadImage(serial);

        [HttpGet(Name = "ControllerTV")]
        public async Task<string> ControllerTV(string serial) =>
             await notificationService.ControllerTV(serial);

        [HttpGet(Name = "AddControllerTV")]
        public async Task<bool> AddControllerTV(string serial, string sendNEC) =>
            await notificationService.AddControllerTV(serial, sendNEC);

        [HttpGet(Name = "ListAllControllerTV")]
        public async Task<string> ListAllControllerTV(string serial) =>
            await notificationService.ListAllControllerTV(serial);

        [HttpGet(Name = "AddDeviceIdNotification")]
        public async Task<bool> AddDeviceIdNotification(string key) =>
            await notificationService.AddDeviceIdNotification(key);
    }
}