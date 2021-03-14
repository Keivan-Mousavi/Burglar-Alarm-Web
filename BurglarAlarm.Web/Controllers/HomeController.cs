using BurglarAlarm.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BurglarAlarm.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> ShowFrameIndex()
        {
            return await Task.Run(() =>
            {
                if (OnlineModel.Frame != null)
                {
                    ViewData.Model =  OnlineModel.Frame.ToString();
                }

                return View();
            });
        }
    }
}
