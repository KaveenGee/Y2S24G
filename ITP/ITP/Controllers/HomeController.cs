using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ITP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ITP.Controllers
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
            int cid = 0;
            String action1, action2, icon, action3, action4 = null;

            if (HttpContext.Session.GetString("customersession") != null)
            {

                cid = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("customersession"));
                action1 = "PROFILE";
                action2 = "LOGOUT";
                icon = "fa-power-off";
                action3 = "userprofile";

            }
            else
            {
                action1 = "LOGIN";
                action2 = "SIGN UP";
                icon = "fa-user-plus";
                action3 = "Customerlogin";
                action4 = "Register";
            }
            ViewData["action3"] = action3;
            ViewData["action4"] = action4;
            ViewData["icon"] = icon;
            ViewData["action1"] = action1;
            ViewData["action2"] = action2;
            ViewData["cid"] = cid;
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
    }
}
