using BurglarAlarm.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BurglarAlarm.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageNotification : ControllerBase
    {
        [HttpPost("Privacy")]
        public async Task<string> Privacy(IFormFile imageFile)
        {
            return await Task.Run(() =>
            {
                try
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
