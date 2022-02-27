using BurglarAlarm.Domain.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BurglarAlarm.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("ShowFrameIndex")]
        public async Task<ActionResult> ShowFrameIndex()
        {
            return await Task.Run(() =>
            {
                if (OnlineModel.Frame != null)
                {
                    ViewData.Model = OnlineModel.Frame.ToString();
                }

                return View();
            });
        }

        [HttpGet]
        public ActionResult TestFaceDetection()
        {
            BurglarAlarm.FaceDetection.FaceDetectionEmguCV.DetectedMultiFace(webHostEnvironment.WebRootPath + "\\img\\0.jpg");

            return Json("Success");
        }
    }
}