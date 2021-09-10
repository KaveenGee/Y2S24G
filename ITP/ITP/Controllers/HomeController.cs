using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        SqlCommand command;
        private static String connectionstring = "workstation id=Project.mssql.somee.com;packet size=4096;user id=donkavi2_SQLLogin_1;pwd=12345678;data source=Project.mssql.somee.com;persist security info=False;initial catalog=Project";
        SqlConnection connection = new SqlConnection(connectionstring);
        SqlDataReader reader;
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext DBob;
       
        public HomeController(AppDbContext DB)
        {
            DBob = DB;
            
        }

        public IActionResult Index()
        {
            
            int cid = 0;
            string action1, action2, icon, action3, action4 = null,action5,img = null;

            if (HttpContext.Session.GetString("customersession") != null)
            {

                cid = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("customersession"));
                img = JsonConvert.DeserializeObject<string>(HttpContext.Session.GetString("customersession_img"));
                if (String.IsNullOrEmpty(img))
                {
                    action5 = "default.png";
                }
                else {
                    action5 = img;
                }
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
                action5 = "plus.png";
            }
            ViewData["action3"] = action3;
            ViewData["action4"] = action4;
            ViewData["icon"] = icon;
            ViewData["action1"] = action1;
            ViewData["action2"] = action2;
            ViewData["cid"] = cid;
            ViewBag.img = action5;
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
