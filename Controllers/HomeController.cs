using HomeService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HomeService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login user)
        {
            string token = "";
            using (var httpclient = new HttpClient())
            {
                httpclient.BaseAddress = new Uri("https://localhost:44336/");
                var postData = httpclient.PostAsJsonAsync<Login>("api/Authentication/AuthenicateUser", user);
                var res = postData.Result;
                if (res.IsSuccessStatusCode)
                {
                    token = await res.Content.ReadAsStringAsync();
                    TempData["token"] = token;
                    if (token != null)
                    {
                        return RedirectToAction("Index", "Booking");
                    }
                }
            }
            return View("Login");

        }
        public IActionResult Index()
        {
            
            return View();
        }
        public IActionResult SentMail()
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("wirenethomeservices@gmail.com");
            msg.To.Add("purushothaman.ramasamy@kanini.com");
            msg.Body = "Testing the automatic mail";
            msg.IsBodyHtml = true;
            msg.Subject = "Test Data";
            SmtpClient smt = new SmtpClient("smtp.gmail.com");
            smt.Port = 25;
            smt.Credentials = new NetworkCredential("wirenethomeservices@gmail.com", "Wirenet@123");
            smt.EnableSsl = false;
            smt.Send(msg);
            return RedirectToAction("Login");
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
    }
}
