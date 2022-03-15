using BurglarAlarm.Domain.Common;
using BurglarAlarm.Domain.Common.AppSettings;
using BurglarAlarm.ExternalService.Contract;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BurglarAlarm.ExternalService
{
    public class NotificationExternalService : INotificationExternalService
    {
        public async Task<bool> SendNotification(string serial, AppSetting appSetting)
        {
            foreach (var deviceId in ListDeviceIdNotification.ListDeviceId)
            {
                WebRequest tRequest = WebRequest.Create(requestUriString: appSetting.Notifications.URL);
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

                tRequest.Headers.Add(string.Format("Authorization: key={0}", appSetting.Notifications.ApplicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", appSetting.Notifications.SenderId));
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = await tRequest.GetRequestStreamAsync())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = await tRequest.GetResponseAsync())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                string sResponseFromServer = await tReader.ReadToEndAsync();
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}
