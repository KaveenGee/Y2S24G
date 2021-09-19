using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITP.Models;
using Microsoft.EntityFrameworkCore;
using ITP.Models.DriverModels;
using Microsoft.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using PasswordGenerator;
using System.Net.Mail;
using System.Text;
using System.Net;

namespace ITP.Controllers
{
    public class DriverController : Controller
    {

        SqlCommand command;
        private static String connectionstring = "workstation id=Project.mssql.somee.com;packet size=4096;user id=donkavi2_SQLLogin_1;pwd=12345678;data source=Project.mssql.somee.com;persist security info=False;initial catalog=Project";
        SqlConnection connection = new SqlConnection(connectionstring);
        SqlDataReader reader;

        private readonly AppDbContext _db;

        
        private readonly IWebHostEnvironment _hostEnvironment;


        public DriverController(AppDbContext db,IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
        }


        //Driver Index
        public IActionResult DriverIndex()
        {
            var displaydata = _db.Driver.ToList();
            return View(displaydata);
        }

        //Driver Create admin
        public IActionResult CreateDriver()
        {
            return View();
        }

    
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateDriver([Bind("Driver_ID,Driver_Name ,username,Password, RePassword ,Driver_Description ,Email,Contact_Number, ImageFile,Vehicle_Type,Vehicle_Number,Vehicle_Capacity")] NewDriverClass ndc)
        {
            if (ModelState.IsValid)
            {
                //Save Image to wwwroot/image
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(ndc.ImageFile.FileName);
                string extension = Path.GetExtension(ndc.ImageFile.FileName);
                ndc.Driver_Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/Driver", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await ndc.ImageFile.CopyToAsync(fileStream);
                }
                //Insert record
                _db.Add(ndc);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(DriverIndex));
            }
            return View(ndc);
        }


        //Driver Create Login
        public IActionResult LogCreateDriver()
        {
            return View();
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> LogCreateDriver([Bind("Driver_ID,Driver_Name ,username,Password ,Driver_Description ,Email,Contact_Number, ImageFile,Vehicle_Type,Vehicle_Number,Vehicle_Capacity")] NewDriverClass ndc)
        {
            if (ModelState.IsValid)
            {
                //Save Image to wwwroot/image
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(ndc.ImageFile.FileName);
                string extension = Path.GetExtension(ndc.ImageFile.FileName);
                ndc.Driver_Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/Driver", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await ndc.ImageFile.CopyToAsync(fileStream);
                }
                //Insert record
                _db.Add(ndc);
                await _db.SaveChangesAsync();
                return View("Driver_Login");
            }
            return View(ndc);
        }



        //Driver Edit Admin
        public async Task<IActionResult> EditDriver(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("DriverIndex");
            }
            var getdriverdetails = await _db.Driver.FindAsync(id);
            return View(getdriverdetails);
        }

        [HttpPost]

        public async Task<IActionResult> EditDriver(NewDriverClass lob)
        {



            command = new SqlCommand("update Driver set Driver_Name = @d,username = @u,Driver_Description = @dd, Email = @e, Contact_Number = @c,Vehicle_Type = @vt, Vehicle_Number=@vn, Vehicle_Capacity=@vc where Driver_ID = @id", connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = lob.Driver_ID;
            command.Parameters.Add("@d", SqlDbType.VarChar, 255).Value = lob.Driver_Name;
            command.Parameters.Add("@u", SqlDbType.VarChar, 255).Value = lob.username;
            command.Parameters.Add("@dd", SqlDbType.VarChar, 1000).Value = lob.Driver_Description;
            command.Parameters.Add("@e", SqlDbType.VarChar, 150).Value = lob.Email;
            command.Parameters.Add("@c", SqlDbType.VarChar, 10).Value = lob.Contact_Number;
            command.Parameters.Add("@vt", SqlDbType.NVarChar, 50).Value = lob.Vehicle_Type;
            command.Parameters.Add("@vn", SqlDbType.NVarChar, 50).Value = lob.Vehicle_Number;
            command.Parameters.Add("@vc", SqlDbType.NVarChar, 50).Value = lob.Vehicle_Capacity;



            connection.Open();
            command.ExecuteNonQuery();
            return RedirectToAction("DriverIndex");
       }



        //Driver Details Admin
        public async Task<IActionResult> DetailsDriver(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("DriverIndex");
            }
            var getdriverdetails = await _db.Driver.FindAsync(id);
            ViewBag.img = getdriverdetails.Driver_Image;
            return View(getdriverdetails);
        }


        //Driver Delete Admin
        public async Task<IActionResult> DeleteDriver(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("DriverIndex");
            }
            var getdriverdetails = await _db.Driver.FindAsync(id);
            return View(getdriverdetails);
        }

        [HttpPost]

        public async Task<IActionResult> DeleteDriver(int id)
        {
           
            var getdriverdetails = await _db.Driver.FindAsync(id);
            _db.Driver.Remove(getdriverdetails);
            await _db.SaveChangesAsync();

            return RedirectToAction("DriverIndex");
        }


        public IActionResult DashboardView()
        {
            return View();
        }

        public IActionResult Methodk()
        {
            var displaydata = _db.Driver.ToList();
            return PartialView("DriverDash",displaydata);
        }


        //Driver Login
        public IActionResult Driver_Login()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> DriverLogin(string UserName, string password)
        {
            command = new SqlCommand("SELECT * FROM Driver where username = @id and Password = @p", connection);
            command.Parameters.Add("@id", SqlDbType.VarChar, 20).Value = UserName;
            command.Parameters.Add("@p", SqlDbType.VarChar, 20).Value = password;
            connection.Open();
            reader = command.ExecuteReader();
            NewDriverClass test = null;
            if (reader.HasRows)

            {
                while (reader.Read())
                {
                    test = new NewDriverClass();
                    test.Driver_ID = int.Parse(reader["Driver_ID"].ToString());




                }
        
                HttpContext.Session.SetString("driversession", JsonConvert.SerializeObject(test.Driver_ID));
                //HttpContext.Session.SetString("customersession_img", JsonConvert.SerializeObject(test.Image));

                var getdriverdetails = await _db.Driver.FindAsync(test.Driver_ID);

                ViewBag.img = getdriverdetails.Driver_Image;

                return View("LogDriverDetails", getdriverdetails);



            }
            else
            {
                return View("Driver_Login");
            }
        }

        //Login Driver Details
        public async Task<IActionResult> LogDriverDetails(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("LogDriverDetails");
            }
            var getdriverdetails = await _db.Driver.FindAsync(id);
            ViewBag.img = getdriverdetails.Driver_Image;
            return View(getdriverdetails);
        }


        //Change profile image
        [HttpPost]
        public async Task<IActionResult> AddProfilePic([Bind("ImageFile")] NewDriverClass ob)
        {

            int did = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("driversession"));
            string rootpath = _hostEnvironment.WebRootPath;
            string picname = Path.GetFileNameWithoutExtension(ob.ImageFile.FileName);
            string extension = Path.GetExtension(ob.ImageFile.FileName);
            picname = picname + extension;
            string path = Path.Combine(rootpath + "/Images/Driver/" + picname);
            using (var filestream = new FileStream(path, FileMode.Create))
            {

                await ob.ImageFile.CopyToAsync(filestream);
            }

            command = new SqlCommand("update Driver set Driver_Image = @pi where Driver_ID = @id", connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = did;
            command.Parameters.Add("@pi", SqlDbType.NVarChar).Value = picname;
            connection.Open();
            command.ExecuteNonQuery();

            var getdriverdetails = await _db.Driver.FindAsync(did);
            ViewBag.img = getdriverdetails.Driver_Image;
            return View("LogDriverDetails",getdriverdetails);
        }


        //Login driver edit
            public async Task<IActionResult> LogDriverEdit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("LogDriverDetails");
            }
            var getdriverdetails = await _db.Driver.FindAsync(id);
            return View(getdriverdetails);
        }


        [HttpPost]

        public async Task<IActionResult> LogDriverEdit(NewDriverClass ob)
        {



            command = new SqlCommand("update Driver set Driver_Name = @d,username = @u,Driver_Description = @dd, Email = @e, Contact_Number = @c,Vehicle_Type = @vt, Vehicle_Number=@vn, Vehicle_Capacity=@vc where Driver_ID = @id", connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = ob.Driver_ID;
            command.Parameters.Add("@d", SqlDbType.VarChar, 255).Value = ob.Driver_Name;
            command.Parameters.Add("@u", SqlDbType.VarChar, 255).Value = ob.username;
            command.Parameters.Add("@dd", SqlDbType.VarChar, 1000).Value = ob.Driver_Description;
            command.Parameters.Add("@e", SqlDbType.VarChar, 150).Value = ob.Email;
            command.Parameters.Add("@c", SqlDbType.VarChar, 10).Value = ob.Contact_Number;
            command.Parameters.Add("@vt", SqlDbType.NVarChar, 50).Value = ob.Vehicle_Type;
            command.Parameters.Add("@vn", SqlDbType.NVarChar, 50).Value = ob.Vehicle_Number;
            command.Parameters.Add("@vc", SqlDbType.NVarChar, 50).Value = ob.Vehicle_Capacity;



            connection.Open();
            command.ExecuteNonQuery();
            return View("Driver_Login");



        }

        public IActionResult DriverForget1()
        {
            return View();
        }

        public IActionResult ForgetDriverEmail(string Email)
        {



            command = new SqlCommand("select Driver_Name,Driver_ID from Driver where Email = @e", connection);
            command.Parameters.Add("@e", SqlDbType.VarChar, 255).Value = Email;
            int cid = 0;
            string name = null;
            connection.Open();
            reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    cid = int.Parse(reader["Driver_ID"].ToString());
                    name = reader["Driver_Name"].ToString();
                }

                var pwd = new Password().IncludeLowercase().IncludeUppercase().LengthRequired(5);
                string result = pwd.Next();
                HttpContext.Session.SetString("did", JsonConvert.SerializeObject(cid));
                //HttpContext.Session.SetString("pass", JsonConvert.SerializeObject(result));



                string Themessage = @"<html>
<body>
<div style="" width: 500px; height: 500px; border: 6px solid midnightblue;position:relative; "">

<img src = cid:myImageID style = "" width:150px;height:auto;position:relative;left:180px;margin-left:170px"">

<h4 style = ""position:relative;left:40px;font-family:Comic Sans MS;margin-left:100px;font-size:20px"" ><font color = ""DodgerBlue""> Hi </font>{name} Welcome to TLC</h4>

<h4 style = ""position:relative;left:190px;top:20px;margin-left:170px;font-size:20px""> Verifivation Code </h4>

<p style = ""font-size:40px;color:#FF7F50;position:relative;left:180px;top:20px;letter-spacing:5px;margin-left:170px"" >{result} </p>



</div>
</body>
</html>";










                Themessage = Themessage.Replace("{result}", result);
                Themessage = Themessage.Replace("{name}", name);
                string to = Email; //To address
                string from = "tlclifepartner2021@gmail.com"; //From address
                MailMessage message = new MailMessage(from, to);
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(Themessage, null, "text/html");




                LinkedResource theEmailImage = new LinkedResource("wwwroot/Images/Driver/driver.png");
                theEmailImage.ContentId = "myImageID";
                htmlView.LinkedResources.Add(theEmailImage);



                LinkedResource theEmailImage2 = new LinkedResource("wwwroot/Images/Driver/driver.png");
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
                return View("DriverForget2");





            }
            else
            {
                ViewBag.indicator = "false";
                return View("DriverForget1");



            }

        }

        public IActionResult DriverVerification(string Verificationcode, string VeriCode)
        {
            if (Verificationcode.Equals(VeriCode))
            {
                return View("DriverForget3");
            }
            else
            {
                ViewBag.pass = Verificationcode;
                return View("DriverForget2");
            }
        }


        public IActionResult DriverNewPwd( string newpwd)
        {
            int aid = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("did"));

            command = new SqlCommand("update Driver set Password = @p,RePassword = @cp where Driver_ID = @id", connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = aid;
            command.Parameters.Add("@p", SqlDbType.NVarChar, 255).Value = newpwd;
            command.Parameters.Add("@cp", SqlDbType.NVarChar, 255).Value = newpwd;
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            return View("Driver_Login");
        }
    }
}
