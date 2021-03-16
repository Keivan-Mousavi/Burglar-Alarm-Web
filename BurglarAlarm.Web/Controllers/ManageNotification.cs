using BurglarAlarm.Web.Component;
using BurglarAlarm.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BurglarAlarm.Web.Controllers
{
    public class ManageNotification : ControllerBase
    {
        [HttpPost(Name = "UploadImage")]
        public async Task<string> UploadImage(IFormFile imageFile)
        {
            return await Task.Run(() =>
            {
                try
                {
                    string serial = HttpContext.Request.Headers["Serial"];

                    var query = WarningListModel.ListModels.Where(w => w.Serial == serial).FirstOrDefault();

                    if (query.StartDate >= DateTime.Now)
                    {
                        OnlineModel.Frame.Clear();
                        OnlineModel.Frame.Append(imageFile.OpenReadStream().ConvertToBase64());

                        return "Success";
                    }
                    else
                    {
                        return "Faild";
                    }
                }
                catch (Exception ex)
                {

                    return "Error";
                }
            });
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
            return await Task.Run(() =>
            {
                try
                {
                    var query = WarningListModel.ListModels.Where(w => w.Serial == serial).FirstOrDefault();

                    if (query != null)
                    {
                        query.StartDate = DateTime.Now.AddMinutes(10);
                        return true;
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

        //private Task SendNotification()
        //{
        //    try
        //    {
        //        var applicationID = "AAAAga5Ng74:APA91bFuHTbk7Nown4COO2agndO3rs_fd_PAwSZKlIcqpfu7llf9_9GSp8F1vRLBkeGoUOYcans54fWP3MW_QsraD6Ne1Nj4KIqCZ8equArzI2tDT44pUhHRGWE1IDFV2-IMiJYcc2C8";
        //        var senderId = "556975096766";

        //        string deviceId = item;

        //        WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
        //        tRequest.Method = "post";
        //        tRequest.ContentType = "application/json";

        //        var data = new
        //        {
        //            to = deviceId,
        //            notification = new
        //            {
        //                body = Body,
        //                title = Title,
        //                icon = "icon"
        //            }
        //        };

        //        var json = JsonConvert.SerializeObject(data);

        //        Byte[] byteArray = Encoding.UTF8.GetBytes(json);

        //        tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
        //        tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
        //        tRequest.ContentLength = byteArray.Length;

        //        using (Stream dataStream = tRequest.GetRequestStream())
        //        {
        //            dataStream.Write(byteArray, 0, byteArray.Length);

        //            using (WebResponse tResponse = tRequest.GetResponse())
        //            {
        //                using (Stream dataStreamResponse = tResponse.GetResponseStream())
        //                {
        //                    using (StreamReader tReader = new StreamReader(dataStreamResponse))
        //                    {
        //                        String sResponseFromServer = tReader.ReadToEnd();
        //                        string str = sResponseFromServer;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
    }
}
