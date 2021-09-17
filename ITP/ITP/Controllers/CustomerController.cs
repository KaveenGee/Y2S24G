using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ITP.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ITP.Controllers
{
    
    public class CustomerController : Controller
    {
       
        SqlCommand command;
        private static String connectionstring = "workstation id=Project.mssql.somee.com;packet size=4096;user id=donkavi2_SQLLogin_1;pwd=12345678;data source=Project.mssql.somee.com;persist security info=False;initial catalog=Project";
        SqlConnection connection = new SqlConnection(connectionstring);
        SqlDataReader reader;
        List<Customer> TestList = new List<Customer>();
        Customer test = null;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly AppDbContext DBob;

        public CustomerController(AppDbContext DB, IWebHostEnvironment hostEnvironment)
        {
            DBob = DB;
            _hostEnvironment = hostEnvironment;
        }

        //method that use to view the registration form
        public IActionResult Register()
        {
            return View();

        }

        //method that use to add new user information to DB
        [HttpPost]
        public async Task<IActionResult> CustomerCreate(Customer ob)
        {
            if (ModelState.IsValid)
            {


                DateTime dt = DateTime.Now;
                DateTime dateOnly = dt.Date;
                ob.Joindate = dateOnly;

               

                DBob.Add(ob);
                await DBob.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View("register", ob);

        }

        //method that use to view the login page
        public IActionResult Customerlogin()
        {
            int cid = 0;
            String action1, action2, icon, action3;

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
            }
            ViewBag.img = "plus.png";
            ViewData["action3"] = action3;
            ViewData["icon"] = icon;
            ViewData["action1"] = action1;
            ViewData["action2"] = action2;
            ViewData["cid"] = cid;
            return View();

        }

        //method that use to validate the user credentials
        [HttpPost]
        public IActionResult userlogin(String username, String password)
        {
            string imageBase64Data = "23";
            command = new SqlCommand("SELECT * FROM CustomerInfo where Username = @id and Password = @p", connection);
            command.Parameters.Add("@id", SqlDbType.VarChar, 20).Value = username;
            command.Parameters.Add("@p", SqlDbType.VarChar, 20).Value = password;
            connection.Open();
            reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    test = new Customer();
                    test.Cusid = int.Parse(reader["Cusid"].ToString());
                    test.FirstName = reader["FirstName"].ToString();
                    test.LastName = reader["LastName"].ToString();
                    test.Email = reader["Email"].ToString();
                    test.Address = reader["Address"].ToString();
                    test.Image = reader["Image"].ToString();
                  

                    TestList.Add(test);
                }
                ViewBag.ob = test;
                HttpContext.Session.SetString("customersession", JsonConvert.SerializeObject(test.Cusid));
                HttpContext.Session.SetString("customersession_img", JsonConvert.SerializeObject(test.Image));
                return RedirectToAction("Index", "Home");

            }
            else
            {
                return View("errorpage");
            }


        }

        //method that use to view the user profile
        public IActionResult userprofile()
        {
            string imageBase64Data = "23";
            int cid = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("customersession"));
            command = new SqlCommand("SELECT * FROM CustomerInfo where Cusid = @id ", connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = cid;

            connection.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                test = new Customer();
                test.Cusid = int.Parse(reader["Cusid"].ToString());
                test.FirstName = reader["FirstName"].ToString();
                test.LastName = reader["LastName"].ToString();
                test.Email = reader["Email"].ToString();
                test.Address = reader["Address"].ToString();
                test.Password = reader["Password"].ToString();
                test.NIC = reader["NIC"].ToString();
                test.PhoneNumber = reader["PhoneNumber"].ToString();
                test.Username = reader["Username"].ToString();
                if (reader["Image"] == DBNull.Value)
                {
                    ViewBag.url = "default.png";
                    ViewBag.img = "default.png";
                }
                else
                {
                    test.Image = reader["Image"].ToString();
                    ViewBag.url = test.Image;
                    ViewBag.img = test.Image;
                }

                TestList.Add(test);
            }
            ViewBag.ob = test;

            int cid1 = 0;
            String action1, action2, icon, action3;

            if (HttpContext.Session.GetString("customersession") != null)
            {

                cid1 = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("customersession"));
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
            }
            ViewData["action3"] = action3;
            ViewData["icon"] = icon;
            ViewData["action1"] = action1;
            ViewData["action2"] = action2;
            ViewData["cid"] = cid1;
            return View();

        }

        //render the partial view1
        public ActionResult render2()
        {
            int cid = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("customersession"));
            command = new SqlCommand("SELECT * FROM CustomerInfo where Cusid = @id", connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = cid;
            connection.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                test = new Customer();
                test.Cusid = int.Parse(reader["Cusid"].ToString());
                test.FirstName = reader["FirstName"].ToString();
                test.LastName = reader["LastName"].ToString();
                test.Username = reader["Username"].ToString();
                test.Password = reader["Password"].ToString();
                test.Email = reader["Email"].ToString();
                test.Address = reader["Address"].ToString();
                test.NIC = reader["NIC"].ToString();
                test.PhoneNumber = reader["PhoneNumber"].ToString();

                TestList.Add(test);
            }
            return PartialView("_cusedit", test);
        }

        //render the partial view 2
        public ActionResult render1()
        {
            int cid = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("customersession"));
            command = new SqlCommand("SELECT * FROM CustomerInfo where Cusid = @c", connection);
            command.Parameters.Add("@c", SqlDbType.Int).Value = cid;

            connection.Open();
            reader = command.ExecuteReader();


            while (reader.Read())
            {
                test = new Customer();
                test.Cusid = int.Parse(reader["Cusid"].ToString());
                test.FirstName = reader["FirstName"].ToString();
                test.LastName = reader["LastName"].ToString();
                test.Email = reader["Email"].ToString();
                test.Address = reader["Address"].ToString();
                test.Password = reader["Password"].ToString();
                test.NIC = reader["NIC"].ToString();
                test.PhoneNumber = reader["PhoneNumber"].ToString();
                test.Username = reader["Username"].ToString();
               
                TestList.Add(test);
            }
            ViewBag.ob = test;
            return PartialView("_cusinfo");



        }

        //method that use to update user information
        [HttpPost]
        public async Task<IActionResult> _cusedit(Customer ob)
        {

            command = new SqlCommand("update CustomerInfo set FirstName = @f,LastName = @l,Email = @e,Address = @a, NIC = @nic, PhoneNumber = @phone,Username = @uname where Cusid = @id", connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = ob.Cusid;
            command.Parameters.Add("@f", SqlDbType.NVarChar, 255).Value = ob.FirstName;
            command.Parameters.Add("@l", SqlDbType.NVarChar, 255).Value = ob.LastName;
            command.Parameters.Add("@e", SqlDbType.NVarChar, 200).Value = ob.Email;
            command.Parameters.Add("@a", SqlDbType.NVarChar, 100).Value = ob.Address;
            command.Parameters.Add("@nic", SqlDbType.NVarChar, 100).Value = ob.NIC;
            command.Parameters.Add("@phone", SqlDbType.NVarChar, 100).Value = ob.PhoneNumber;
            command.Parameters.Add("@uname", SqlDbType.NVarChar, 100).Value = ob.Username;

            connection.Open();
            command.ExecuteNonQuery();
            int cid = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("customersession"));
            Customer d = await DBob.CustomerInfo.FindAsync(cid);
            
            ViewBag.ob = d;
            ViewData["action1"] = "PROFILE";
            ViewData["action2"] = "LOGOUT";
            if (String.IsNullOrEmpty(d.Image))
            {
                ViewBag.img = "default.png";
                ViewBag.url = "default.png";
            }
            else
            {
                ViewBag.img = d.Image;
                ViewBag.url = d.Image;
            }
            return View("userprofile");

        }


        //method that use to chage the profile picture
        [HttpPost]
        public async Task<IActionResult> AddProfilePic([Bind("Imagefile")] Customer ob)
        {

            int cid = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("customersession"));
            string rootpath = _hostEnvironment.WebRootPath;
            string picname = Path.GetFileNameWithoutExtension(ob.Imagefile.FileName);
            string extension = Path.GetExtension(ob.Imagefile.FileName);
            picname = picname + extension;
            string path = Path.Combine(rootpath + "/Images/Customer/" + picname);
            using (var filestream = new FileStream(path, FileMode.Create))
            {

                await ob.Imagefile.CopyToAsync(filestream);
            }
            command = new SqlCommand("update CustomerInfo set Image = @pi where Cusid = @id", connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = cid;
            command.Parameters.Add("@pi", SqlDbType.VarChar).Value = picname;
            connection.Open();
            command.ExecuteNonQuery();

            command = new SqlCommand("SELECT * FROM CustomerInfo where Cusid = @c", connection);
            command.Parameters.Add("@c", SqlDbType.Int).Value = cid;

            reader = command.ExecuteReader();


            while (reader.Read())
            {
                test = new Customer();
                test.Cusid = int.Parse(reader["Cusid"].ToString());
                test.FirstName = reader["FirstName"].ToString();
                test.LastName = reader["LastName"].ToString();
                test.Email = reader["Email"].ToString();
                test.Address = reader["Address"].ToString();
                test.Password = reader["Password"].ToString();
                test.NIC = reader["NIC"].ToString();
                test.PhoneNumber = reader["PhoneNumber"].ToString();
                test.Username = reader["Username"].ToString();
                test.Image = reader["image"].ToString();
                ViewBag.url = test.Image;
                ViewBag.img = test.Image;
                HttpContext.Session.SetString("customersession_img", JsonConvert.SerializeObject(test.Image));
                TestList.Add(test);
                
            }
            ViewBag.ob = test;

            int cid1 = 0;
            String action1, action2, icon, action3;

            if (HttpContext.Session.GetString("customersession") != null)
            {

                cid1 = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("customersession"));
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
            }
            ViewData["action3"] = action3;
            ViewData["icon"] = icon;
            ViewData["action1"] = action1;
            ViewData["action2"] = action2;
            ViewData["cid"] = cid1;
            return View("userprofile");


        }

        //method use to delete user profile by user
        public async Task<IActionResult> deleteuser(int id)
        {

            int cid = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("customersession"));
            command = new SqlCommand("Delete CustomerInfo where Cusid = @id", connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = cid;
            connection.Open();
            command.ExecuteNonQuery();
            return RedirectToAction("index", "Home");
        }

    

    }
}
