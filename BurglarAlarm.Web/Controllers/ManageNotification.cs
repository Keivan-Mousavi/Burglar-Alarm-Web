using BurglarAlarm.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
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
                        DateTime dt = DateTime.Now;

                        string name = dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString() + dt.Millisecond.ToString();

                        string ImageName = name + Path.GetExtension(imageFile.FileName);

                        string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", ImageName);

                        using (var stream = new FileStream(SavePath, FileMode.Create))
                        {
                            imageFile.CopyTo(stream);
                        }

                        return "Success";
                    }
                    else
                    {
                        return "Faild";
                    }
                }
                catch
                {
                    return "Faild";
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
                       
                    }
                    else
                    {
                        
                    }

                    return "";
                }
                catch (Exception ex)
                {
                    return "";
                }
            });
        }
    }
}
