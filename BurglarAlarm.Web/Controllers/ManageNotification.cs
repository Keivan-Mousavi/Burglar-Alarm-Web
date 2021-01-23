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
                    string sp = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "header.txt");
                    string header = JsonConvert.SerializeObject(HttpContext.Request.Headers);
                    System.IO.File.WriteAllText(sp, header);

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
                        query.Flag = true;

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
    }
}
