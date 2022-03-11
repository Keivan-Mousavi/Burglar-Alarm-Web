﻿using BurglarAlarm.Domain.Common;
using BurglarAlarm.ExternalService.Contract;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using BurglarAlarm.Domain.Common.AppSettings;

namespace BurglarAlarm.ExternalService
{
    public class NotificationExternalService : INotificationExternalService
    {
        public async Task<bool> SendNotification(string serial)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var configuration = new ConfigurationBuilder().SetBasePath("appsettings.json").Build().Get<AppSetting>();

                    foreach (var item in ListDeviceIdNotification.ListDeviceId)
                    {
                        //var applicationID = "AAAAMxG6zms:APA91bGRhmkwYGbSkNidY3LaktNzIl9pmECjZJXY64R_i_7lFCE3f2-8xzN-VTwbpqValmNImz7-scqm2GEZACmayWzY5gNVqYg4qNuc4lfTv6XhQMF51BCfnXULYESWZTY42C1Yb01J";
                        //var senderId = "219340787307";

                        string deviceId = item;

                        WebRequest tRequest = WebRequest.Create(requestUriString: configuration.Notifications.URL);
                        tRequest.Method = "post";
                        tRequest.ContentType = "application/json";

                        var data = new
                        {
                            to = deviceId,
                            notification = new
                            {
                                body = "Security systems activated. Your TV is on",
                                title = "Warning",
                                icon = "icon"
                            }
                        };

                        var json = JsonSerializer.Serialize(data);

                        Byte[] byteArray = Encoding.UTF8.GetBytes(json);

                        tRequest.Headers.Add(string.Format("Authorization: key={0}", configuration.Notifications.ApplicationID));
                        tRequest.Headers.Add(string.Format("Sender: id={0}", configuration.Notifications.SenderId));
                        tRequest.ContentLength = byteArray.Length;

                        using (Stream dataStream = tRequest.GetRequestStream())
                        {
                            dataStream.Write(byteArray, 0, byteArray.Length);

                            using (WebResponse tResponse = tRequest.GetResponse())
                            {
                                using (Stream dataStreamResponse = tResponse.GetResponseStream())
                                {
                                    using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                    {
                                        string sResponseFromServer = tReader.ReadToEnd();
                                    }
                                }
                            }
                        }
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
