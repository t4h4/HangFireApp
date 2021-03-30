using HangFire.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HangFire.Web.BackgroundJobs;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace HangFire.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SignUp()
        {
            // üye kayıt işlemi bu method da gerçekleşiyor.
            // yeni üye olan kullanıcının user ID al.
            FireAndForgetJobs.EmailSendToUserJob("1234","sitemize hoş geldiniz.");

            return View();
        }

        public IActionResult PictureSave()
        {
            BackgroundJobs.RecurringJobs.ReportingJob();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PictureSave(IFormFile picture)
        {
            string newFileName = String.Empty;

            if (picture != null && picture.Length > 0)
            {
                newFileName = Guid.NewGuid().ToString() + Path.GetExtension(picture.FileName); //guid ile rastgele dosya ismi oluşturduk. dosya uzantısını ise Path.GetExtension ile ayarladık.

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pictures", newFileName); //kaydedeceğimiz path'i belirliyoruz.

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await picture.CopyToAsync(stream); //kullanıcıdan aldığım resmi kopyaladım.
                }

                string jobID = BackgroundJobs.DelayedJobs.AddWaterMarkJob(newFileName, "TAHA");
            }

            return View();
        }
    }
}
