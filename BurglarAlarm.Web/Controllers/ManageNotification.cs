using BurglarAlarm.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurglarAlarm.Web.Controllers
{
    [Route("ManageNotification")]
    [ApiController]
    public class ManageNotification : ControllerBase
    {
        [HttpPost("UploadImage")]
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
                        using (var memoryStream = ((MemoryStream)imageFile.OpenReadStream()))
                        {
                            OnlineModel.Frame.Append(Convert.ToBase64String(memoryStream.ToArray()));
                        }

                        //DateTime dt = DateTime.Now;

                        //string name = dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString() + dt.Millisecond.ToString();

                        //string ImageName = name + Path.GetExtension(imageFile.FileName);

                        //string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", ImageName);

                        //using (var stream = new FileStream(SavePath, FileMode.Create))
                        //{
                        //    imageFile.CopyTo(stream);
                        //}

                        return "Success";
                    }
                    else
                    {
                        return "Faild";
                    }
                }
                catch
                {
                    return "Error";
                }
            });
        }

        [HttpGet("ShowFrame")]
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

        [HttpGet("CheckCamera")]
        public async Task<bool> CheckCamera(string serial)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var query = WarningListModel.ListModels.Where(w => w.Serial == serial).FirstOrDefault();

                    if (query.Serial == serial)
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

        [HttpGet("ControllerTV")]
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

        [HttpGet("AddControllerTV")]
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
    }
}
