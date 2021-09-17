using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ITP.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PasswordGenerator;

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
            String action1, action2, icon, action3,action4 = null;

            if (HttpContext.Session.GetString("customersession") != null)
            {

                cid1 = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("customersession"));
                action1 = "PROFILE";
                action2 = "LOGOUT";
                icon = "fa-power-off";
                action3 = "userprofile";
                action4 = "logout";

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
            ViewData["icon"] = icon;
            ViewData["action1"] = action1;
            ViewData["action2"] = action2;
            ViewData["action4"] = action4;
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
            ViewBag.ob = test;
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
                test.Image = reader["Image"].ToString();


            }


            ViewBag.ob = test;
            int cid1 = 0;
            String action1, action2, icon, action3, action4 = null;

            if (HttpContext.Session.GetString("customersession") != null)
            {

                cid1 = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("customersession"));
                action1 = "PROFILE";
                action2 = "LOGOUT";
                icon = "fa-power-off";
                action3 = "userprofile";
                action4 = "logout";

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
            ViewData["icon"] = icon;
            ViewData["action1"] = action1;
            ViewData["action2"] = action2;
            ViewData["action4"] = action4;
            ViewData["cid"] = cid1;
            if (String.IsNullOrEmpty(test.Image))
            {
                ViewBag.img = "default.png";
                ViewBag.url = "default.png";
            }
            else
            {
                ViewBag.img = test.Image;
                ViewBag.url = test.Image;
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
            String action1, action2, icon, action3,action4 = null;

            if (HttpContext.Session.GetString("customersession") != null)
            {

                cid1 = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("customersession"));
                action1 = "PROFILE";
                action2 = "LOGOUT";
                icon = "fa-power-off";
                action3 = "userprofile";
                action4 = "logout";

            }
            else
            {
                action1 = "LOGIN";
                action2 = "SIGN UP";
                icon = "fa-user-plus";
                action3 = "Customerlogin";
                action4 = "logout";
            }
            ViewData["action3"] = action3;
            ViewData["icon"] = icon;
            ViewData["action1"] = action1;
            ViewData["action2"] = action2;
            ViewData["action4"] = action4;
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

        public async Task<IActionResult> editpassword(string cp, string np, string npa)
        {
            int cid = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("customersession"));
            command = new SqlCommand("update CustomerInfo set Password = @p,ConfirmPassword = @cp where Cusid = @id", connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = cid;
            command.Parameters.Add("@p", SqlDbType.NVarChar, 255).Value = np;
            command.Parameters.Add("@cp", SqlDbType.NVarChar, 255).Value = np;
            connection.Open();
            command.ExecuteNonQuery();
            return RedirectToAction("userprofile");

        }

       
        //method that use to send email to the user
        [HttpPost]
        public IActionResult setEmail(string emailin) {

            command = new SqlCommand("select FirstName,Cusid from customerinfo where Email = @e", connection);
            command.Parameters.Add("@e", SqlDbType.NVarChar, 255).Value = emailin;
            int cid = 0;
            string name = null;
            connection.Open();
            reader = command.ExecuteReader();
            if (reader.HasRows) {
                while (reader.Read())
                {
                     cid = int.Parse(reader["Cusid"].ToString());
                    name = reader["FirstName"].ToString();
                }
              
                var pwd = new Password().IncludeLowercase().IncludeUppercase().LengthRequired(5);
                string result = pwd.Next();
                HttpContext.Session.SetString("cid", JsonConvert.SerializeObject(cid));
                HttpContext.Session.SetString("pass", JsonConvert.SerializeObject(result)); 

                string Themessage = @"<html>
                          <body>
                            <div style="" width: 500px; height: 620px; border: 6px solid DodgerBlue;position:relative; "">

                <img src = cid:myImageID style = "" width:150px;height:auto;position:relative;left:180px;margin-left:170px"">
        

                 <h4 style = ""position:relative;left:40px;font-family:Comic Sans MS;margin-left:100px;font-size:20px"" ><font color = ""DodgerBlue""> Hi </font>{name} your Password is Reseted </h4>
                  
                         <img src = cid:myImageID2 style = "" width:270px;height:250px;position:absolute;left:300px;border-radius:50%;margin-left:120px"">
                     
                            <h4 style = ""position:relative;left:190px;top:20px;margin-left:170px;font-size:20px""> New Password </h4>
                          
                              <p style = ""font-size:40px;color:#FF7F50;position:relative;left:180px;top:20px;letter-spacing:5px;margin-left:170px"" >{result} </p>
                           

                           </div>
                            </body>
                            </html>";








                Themessage = Themessage.Replace("{result}", result);
                Themessage = Themessage.Replace("{name}", name);
                string to = emailin; //To address    
                string from = "tlclifepartner2021@gmail.com"; //From address    
                MailMessage message = new MailMessage(from, to);
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(Themessage, null, "text/html");


                LinkedResource theEmailImage = new LinkedResource("wwwroot/Images/Customer/logo.png");
                theEmailImage.ContentId = "myImageID";
                htmlView.LinkedResources.Add(theEmailImage);

                LinkedResource theEmailImage2 = new LinkedResource("wwwroot/Images/Customer/re.gif");
                theEmailImage2.ContentId = "myImageID2";
                htmlView.LinkedResources.Add(theEmailImage2);

                message.AlternateViews.Add(htmlView);

                message.Subject = "Sending Email Using Asp.Net & C#";
                //message.Body = mailbody;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
                NetworkCredential MyCredentials = new NetworkCredential("tlclifepartner2021@gmail.com", "Tlc@2021");

                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = MyCredentials;


                client.Send(message);
                ViewBag.pass = result;
                ViewBag.cid = cid;
                return View("ForgetPassword2");



            }
            else
            {
                ViewBag.indicator = "false";
                return View("ForgetPassword1");

            }
          
            
        }

        //method that use to view the fogetpassword page
        public IActionResult ForgetPassword2()
        {
            ViewBag.indicator = "true";
            return View();
        }

        //method that use to view the fogetpassword page
        public IActionResult ForgetPassword1()
        {
            ViewBag.indicator = "true";
            return View();
        }

        //method that use to view the fogetpassword page
        public IActionResult ForgetPassword3()
        {
           
            return View();
        }

        //method that use to compare the code that send throught email
        public IActionResult verifypassword(string pass, string inp1, string inp2, string inp3, string inp4, string inp5,string cid)
        {
            string getpass = inp1 + inp2 + inp3 + inp4 + inp5;
            if (pass.Equals(getpass))
            {
                ViewBag.cid = cid;
                return View("ForgetPassword3");

            }
            else
            {
                string pass1 = JsonConvert.DeserializeObject<string>(HttpContext.Session.GetString("pass"));
                ViewBag.pass = pass1;
                ViewBag.indicator = "false";
                return View("ForgetPassword2");

            }

        }

        //method that use to recover the password
        public IActionResult savenewPassword(string np,string npa,string getcid)
        {
            command = new SqlCommand("update CustomerInfo set Password = @p,ConfirmPassword = @cp where Cusid = @id", connection);
            int cid = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("cid"));
            
            command.Parameters.Add("@id", SqlDbType.Int).Value = cid;
            command.Parameters.Add("@p", SqlDbType.NVarChar, 255).Value = np;
            command.Parameters.Add("@cp", SqlDbType.NVarChar, 255).Value = np;
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            return RedirectToAction("Customerlogin");
        }


        //method  that use to logout
        public IActionResult logout()
        {
            HttpContext.Session.Remove("customersession");
            HttpContext.Session.Remove("customersession_img");
            return RedirectToAction("Index","Home");
        }
    }
}
