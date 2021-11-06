using BurglarAlarm.Domain.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BurglarAlarm.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

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
                    ViewData.Model = OnlineModel.Frame.ToString();
                }

                return View();
            });
        }

        [HttpGet]
        public ActionResult TestFaceDetection()
        {
            BurglarAlarm.FaceDetection.FaceDetectionEmguCV.DetectedMultiFace(hostingEnvironment.WebRootPath + "\\img\\0.jpg");

            return Json("Success");
        }
    }
}
