using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PasswordGenerator;

namespace ITP.Controllers
{
    public class TLC_AdminController : Controller
    {
        SqlCommand command;
        private static String connectionstring = "workstation id=Project.mssql.somee.com;packet size=4096;user id=donkavi2_SQLLogin_1;pwd=12345678;data source=Project.mssql.somee.com;persist security info=False;initial catalog=Project";
        SqlConnection connection = new SqlConnection(connectionstring);
        SqlDataReader reader;
        List<Customer> TestList = new List<Customer>();
        Customer test = null;
        private readonly AppDbContext DBob;
        private readonly IWebHostEnvironment _hostEnvironment;
        public TLC_AdminController(AppDbContext DB, IWebHostEnvironment hostEnvironment)
        {
            DBob = DB;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
           
            return View();
        }

        //method that use to load the owner dashboard
        public IActionResult Owner_dashboard()
        {
            
            return View("Owner/Owner_dashboard");
        }

     

        //render the owner dashboard partial view1
        public IActionResult _dashboard()
        {
            string name = JsonConvert.DeserializeObject<String>(HttpContext.Session.GetString("nam"));

            string img = JsonConvert.DeserializeObject<String>(HttpContext.Session.GetString("Adminsession_img"));
            int ic = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("ic"));
            int cc = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("cc"));
            int oc = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("oc"));
            int dc = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("dc"));
            int fc = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("fc"));
            int ec = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("ec"));

            int sc = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("sc"));
            int moc = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("moc"));

            ViewBag.itemcount = ic;
            ViewBag.cuscount = cc;
            ViewBag.ocount = oc;
            ViewBag.dcount = dc;
            ViewBag.fcount = fc;
            ViewBag.mocount = moc;
            ViewBag.scount = sc;
            ViewBag.ecount = ec;
            DateTime now = DateTime.Now;
            string s = now.DayOfWeek.ToString();
            ViewBag.day = s;
            ViewBag.name = name;
            ViewBag.TerminoDaAvaliacao = now;

            return PartialView("Owner/_dashboard");
        }

       

        //render the owner dashboard partial view2
        public ActionResult CustomerList()
        {
            string img = JsonConvert.DeserializeObject<string>(HttpContext.Session.GetString("Adminsession_img"));
            command = new SqlCommand("select * from CustomerInfo", connection);
            // command.CommandType = System.Data.CommandType.TableDirect;
            connection.Open();
            reader = command.ExecuteReader();



            while (reader.Read())
            {
                test = new Customer();

                test.FirstName = reader["FirstName"].ToString();
                test.LastName = reader["LastName"].ToString();
                test.Email = reader["Email"].ToString();
                test.Address = reader["Address"].ToString();
                test.Cusid = int.Parse(reader["Cusid"].ToString());
                test.NIC = reader["NIC"].ToString();
                test.PhoneNumber = reader["PhoneNumber"].ToString();
                test.Image = reader["Image"].ToString();
                if (test.Image == null) {
                    test.Image = "default.png";
                }
                TestList.Add(test);
            }
            connection.Close();
            ViewBag.url = img;
          ViewBag.totalcustomer = TestList;
            return View("Owner/CustomerList");
        }


      

        //method for add new customer by admin modal
        [HttpPost]
        public ActionResult saveCustomer(Customer ob)
        {

          
            var pwd = new Password().IncludeLowercase().IncludeUppercase().LengthRequired(5);
            string result = pwd.Next();
            var uname = new Password().IncludeLowercase().LengthRequired(6);
            string runame = uname.Next();
            DateTime dt = DateTime.Now;
            DateTime dateOnly = dt.Date;
            ob.Joindate = dateOnly;
            ob.Username = runame;
            ob.Password = result;
            ob.ConfirmPassword = result;
            DBob.CustomerInfo.Add(ob);
            DBob.SaveChanges();


            string Themessage = @"<html>
                          <body>
                            <div style="" width: 500px; height: 620px; border: 6px solid DodgerBlue;position:relative; "">

                <img src = cid:myImageID style = "" width:150px;height:auto;position:relative;left:180px;margin-left:170px"">
        

                 <h4 style = ""position:relative;left:40px;font-family:Comic Sans MS;margin-left:60px;font-size:20px"" ><font color = ""DodgerBlue""> Hi </font>{name} we Created an Account for you </h4>
                  
                         <img src = cid:myImageID2 style = "" width:270px;height:250px;position:absolute;left:300px;border-radius:50%;margin-left:120px"">
                     
                            <h4 style = ""position:relative;left:90px;top:0px;margin-left:170px;font-size:20px""> Username = <font color = ""#9932CC"">{uname}</font> </h4>
                            <h4 style = ""position:relative;left:90px;top:0px;margin-left:170px;font-size:20px""> Password = <font color = ""#9932CC"">{pword}</font> </h4>
                          
                              <p style = ""font-size:20px;color:#FF7F50;position:relative;left:80px;top:0px;letter-spacing:5px;margin-left:70px"" >WELCOME TO TLC FAMILY </p>
                           

                           </div>
                            </body>
                            </html>";







           
            Themessage = Themessage.Replace("{uname}", runame);
            Themessage = Themessage.Replace("{pword}", result);
            Themessage = Themessage.Replace("{name}", ob.FirstName);
            string to = ob.Email; //To address    
            string from = "tlclifepartner2021@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, to);
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(Themessage, null, "text/html");


            LinkedResource theEmailImage = new LinkedResource("wwwroot/Images/Customer/logo.png");
            theEmailImage.ContentId = "myImageID";
            htmlView.LinkedResources.Add(theEmailImage);

            LinkedResource theEmailImage2 = new LinkedResource("wwwroot/Images/Customer/rocket.gif");
            theEmailImage2.ContentId = "myImageID2";
            htmlView.LinkedResources.Add(theEmailImage2);

            message.AlternateViews.Add(htmlView);

            message.Subject = "Welcome To TLC Family";
            //message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            NetworkCredential MyCredentials = new NetworkCredential("tlclifepartner2021@gmail.com", "Tlc@2021");

            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = MyCredentials;


            client.Send(message);


            return RedirectToAction("CustomerList");
        }

        //method for delete customer by admin modal
        [HttpPost]
        public ActionResult Delete_byadmin(int id)
        {
            command = new SqlCommand("delete CustomerInfo where Cusid = @id", connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;
            // command.CommandType = System.Data.CommandType.TableDirect;
            connection.Open();
            command.ExecuteNonQuery();
            return RedirectToAction("CustomerList");

        }

        //method for edit customer by admin modal popup
        public ActionResult Editcusdata(Customer ob)
        {

            command = new SqlCommand("update CustomerInfo set FirstName = @f,LastName = @l,Email = @e,Address = @a,NIC = @n,PhoneNumber = @pn where Cusid = @id", connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = ob.Cusid;
            command.Parameters.Add("@f", SqlDbType.NVarChar, 255).Value = ob.FirstName;
            command.Parameters.Add("@l", SqlDbType.NVarChar, 255).Value = ob.LastName;
            command.Parameters.Add("@e", SqlDbType.NVarChar, 200).Value = ob.Email;
            command.Parameters.Add("@a", SqlDbType.NVarChar, 100).Value = ob.Address;
            command.Parameters.Add("@n", SqlDbType.NVarChar, 100).Value = ob.NIC;
            command.Parameters.Add("@pn", SqlDbType.NVarChar, 100).Value = ob.PhoneNumber;
            connection.Open();
            command.ExecuteNonQuery();

            return RedirectToAction("CustomerList");


        }

        //method that use to implemet search function
        [HttpGet]
        public async Task<IActionResult> CustomerList2(string cusSearch) {
            ViewData["cusdetails"] = cusSearch;
            command = new SqlCommand("select * from  CustomerInfo where FirstName like @key", connection);
            cusSearch = "%" + cusSearch + "%";
            command.Parameters.Add("@key", SqlDbType.VarChar).Value = cusSearch;

            
            connection.Open();
            reader = command.ExecuteReader();



            while (reader.Read())
            {
                test = new Customer();

                test.FirstName = reader["FirstName"].ToString();
                test.LastName = reader["LastName"].ToString();
                test.Email = reader["Email"].ToString();
                test.Address = reader["Address"].ToString();
                test.Cusid = int.Parse(reader["Cusid"].ToString());
                test.NIC = reader["NIC"].ToString();
                test.PhoneNumber = reader["PhoneNumber"].ToString();
                test.Image = reader["Image"].ToString();
                if (test.Image == null)
                {
                    test.Image = "default.png";
                }
                TestList.Add(test);
            }
            string img = JsonConvert.DeserializeObject<string>(HttpContext.Session.GetString("Adminsession_img"));
            connection.Close();
            ViewBag.url = img;
            ViewBag.totalcustomer = TestList;
            return View("Owner/CustomerList");
         
           
           

            
        }

        //reder customer partial view
        public IActionResult _customer() {
            command = new SqlCommand("SELECT * FROM CustomerInfo", connection);
            connection.Open();
            reader = command.ExecuteReader();
            int ccount = reader.Cast<object>().Count();
            
           
            ViewBag.c = ccount;
            return PartialView("Owner/_customer");
        }


        //method for draw the customer registration graph
        public JsonResult draw()
        {
            DateTime now = DateTime.Now;
            string monthName = null;
            List<Join> ob = new List<Join>();

            int cmonth = now.Month;


            for (int i = 1; i <= cmonth; i++)
            {
                command = new SqlCommand("select *  from Customerinfo where  datepart(month,Joindate) = @m", connection);
                command.Parameters.Add("@m", SqlDbType.Int).Value = i;
                connection.Open();
                Join ob1 = new Join();
                reader = command.ExecuteReader();
                monthName = new DateTimeFormatInfo().GetMonthName(i);
                ob1.month = monthName;
                ob1.count = reader.Cast<object>().Count();
                ob.Add(ob1);
                connection.Close();
            }
            var data = ob;

            return Json(data);


        }


        
        //method for admin login
        public IActionResult ManagerLogin(String username, String password,String type1)
        {
            String page = null;
            Admin test = null;
            if (type1.Equals("HR"))
            {
                type1 = "HR Manager";
                

            }
            else if (type1.Equals("IM")) {
                type1 = "Inventory Manager";
                page = "Item/DashSample";
            }
            else if (type1.Equals("DM"))
            {
                type1 = "Delivery Manager";
            }
            else if (type1.Equals("SM"))
            {
                type1 = "Supplier Manager";
            }
            else if (type1.Equals("OW"))
            {
                type1 = "Owner";
                page = "Owner/Owner_dashboard";

            }

            command = new SqlCommand("SELECT * FROM Admin where Username = @id and Password = @p and Type = @t", connection);
            command.Parameters.Add("@id", SqlDbType.VarChar, 255).Value = username;
            command.Parameters.Add("@p", SqlDbType.VarChar, 255).Value = password;
            command.Parameters.Add("@t", SqlDbType.VarChar, 20).Value = type1;
            connection.Open();
            reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    test = new Admin();
                    test.Admin_ID = int.Parse(reader["Admin_ID"].ToString());
                    test.Image = reader["Image"].ToString();
                    test.Name = reader["Name"].ToString();


                    
                }
                ViewBag.ob = test;
                HttpContext.Session.SetString("Adminsession", JsonConvert.SerializeObject(test.Admin_ID));
                HttpContext.Session.SetString("Adminsession_img", JsonConvert.SerializeObject(test.Image));
                connection.Close();

                string imgp = null;
                
                if (String.IsNullOrEmpty(test.Image))
                {
                    imgp = "default.png";
                }
                else
                {
                    imgp = test.Image;
                }
                DateTime now = DateTime.Now;
                int cmonth = now.Month;

                command = new SqlCommand("SELECT * FROM item", connection);
                connection.Open();
                reader = command.ExecuteReader();
                int icount = reader.Cast<object>().Count();
                reader.Close();

                command = new SqlCommand("SELECT * FROM CustomerInfo", connection);
                reader = command.ExecuteReader();
                int ccount = reader.Cast<object>().Count();
                reader.Close();

                command = new SqlCommand("SELECT * FROM orders", connection);
                reader = command.ExecuteReader();
                int ocount = reader.Cast<object>().Count();
                reader.Close();

                command = new SqlCommand("SELECT * FROM driver", connection);
                reader = command.ExecuteReader();
                int dcount = reader.Cast<object>().Count();
                reader.Close();

                command = new SqlCommand("SELECT * FROM Employee", connection);
                reader = command.ExecuteReader();
                int ecount = reader.Cast<object>().Count();
                reader.Close();


               

                command = new SqlCommand("SELECT * FROM Supplierlog", connection);
                reader = command.ExecuteReader();
                int scount = reader.Cast<object>().Count();
                reader.Close();

                command = new SqlCommand("SELECT TotalPrice FROM Orders o, Orderdetails od where o.id = od.OrderId and  datepart(month,OrderDate) = @m ", connection);
                command.Parameters.Add("@m", SqlDbType.Int).Value = cmonth;
                reader = command.ExecuteReader();
                int total = 0;
                while (reader.Read()) { 
                    int value = int.Parse(reader["TotalPrice"].ToString());
                    total = total + value;
                }
                reader.Close();



                command = new SqlCommand("SELECT * FROM feedback", connection);
                reader = command.ExecuteReader();
                int fcount = reader.Cast<object>().Count();


                HttpContext.Session.SetString("ic", JsonConvert.SerializeObject(icount));
                HttpContext.Session.SetString("nam", JsonConvert.SerializeObject(test.Name));
                HttpContext.Session.SetString("moc", JsonConvert.SerializeObject(total));
                HttpContext.Session.SetString("sc", JsonConvert.SerializeObject(scount));
              
                HttpContext.Session.SetString("ec", JsonConvert.SerializeObject(ecount));
                HttpContext.Session.SetString("cc", JsonConvert.SerializeObject(ccount));
                HttpContext.Session.SetString("oc", JsonConvert.SerializeObject(ocount));
                HttpContext.Session.SetString("dc", JsonConvert.SerializeObject(dcount));
                HttpContext.Session.SetString("fc", JsonConvert.SerializeObject(fcount));
                ViewBag.itemcount = icount;
                ViewBag.cuscount = ccount;
                ViewBag.ocount = ocount;
                ViewBag.dcount = dcount;
                ViewBag.fcount = fcount;
                ViewBag.mocount = total;
                ViewBag.scount = scount;
                
                ViewBag.ecount = ecount;
                ViewBag.mpic = imgp;
               
                string s = now.DayOfWeek.ToString();
                ViewBag.day = s;
                ViewBag.name = test.Name;
                ViewBag.TerminoDaAvaliacao = now;
                connection.Close();
                return View(page);
            
               
            }
            else
            {
                return View("errorpage");
            }
        }

        public IActionResult Managerprofile() {
            Admin test = null;
            
            int aid = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("Adminsession"));
            command = new SqlCommand("SELECT * FROM Admin where Admin_ID = @id ", connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = aid;

            connection.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                test = new Admin();
                test.Admin_ID = int.Parse(reader["Admin_ID"].ToString());
                test.Name = reader["Name"].ToString();
                test.Username = reader["Username"].ToString();
                test.Password = reader["Password"].ToString();
                test.Email = reader["Email"].ToString();
                test.Phone_Number = reader["Phone_Number"].ToString();
                test.NIC = reader["NIC"].ToString();
                test.Type = reader["Type"].ToString();

                
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

               
            }
            ViewBag.ob = test;
            return View("Owner/Managerprofile");

        }

        //method that use to render the managerinfo partial view
        public async Task<IActionResult> render1()
        {
            int cid = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("Adminsession"));
            var model = await DBob.Admin.FindAsync(cid);
            ViewBag.ob = model;
            return PartialView("Owner/_managerinfo");
        }

        //method that use to render the manageredit partial view
        public async Task<IActionResult> render2()
        {
            int cid = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("Adminsession"));
            var model = await DBob.Admin.FindAsync(cid);
            ViewBag.ob = model;
            return PartialView("Owner/_manageredit");
        }


        //method that use to edit manager info
        [HttpPost]
        public async Task<IActionResult> _manageredit(Admin ob)
        {

            command = new SqlCommand("update Admin set Name = @f,Username = @l,Email = @e, NIC = @nic, Phone_Number = @phone where Admin_ID= @id", connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = ob.Admin_ID;
            command.Parameters.Add("@f", SqlDbType.NVarChar, 255).Value = ob.Name;
            command.Parameters.Add("@l", SqlDbType.NVarChar, 255).Value = ob.Username;
            command.Parameters.Add("@e", SqlDbType.NVarChar, 200).Value = ob.Email;
            
            command.Parameters.Add("@nic", SqlDbType.NVarChar, 100).Value = ob.NIC;
            command.Parameters.Add("@phone", SqlDbType.NVarChar, 100).Value = ob.Phone_Number;
            

            connection.Open();
            command.ExecuteNonQuery();
            Admin d = await DBob.Admin.FindAsync(ob.Admin_ID);

            ViewBag.ob = d;
            if (String.IsNullOrEmpty(d.Image))
            {
                ViewBag.url = "default.png";
                ViewBag.img = "default.png";
            }
            else
            {
               
                ViewBag.url = d.Image;
                ViewBag.img = d.Image;
            }

            
            return View("Owner/Managerprofile");

        }


        //method use to add profile pic to admin
        [HttpPost]
        public async Task<IActionResult> AddProfilePic([Bind("Imagefile")] Admin ob)
        {
            Admin d = null;
            int cid = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("Adminsession"));
            string rootpath = _hostEnvironment.WebRootPath;
            string picname = Path.GetFileNameWithoutExtension(ob.Imagefile.FileName);
            string extension = Path.GetExtension(ob.Imagefile.FileName);
            picname = picname + extension;
            string path = Path.Combine(rootpath + "/Images/Manager/" + picname);
            using (var filestream = new FileStream(path, FileMode.Create))
            {

                await ob.Imagefile.CopyToAsync(filestream);
            }
            command = new SqlCommand("update Admin set Image = @pi where Admin_ID = @id", connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = cid;
            command.Parameters.Add("@pi", SqlDbType.VarChar).Value = picname;
            HttpContext.Session.SetString("Adminsession_img", JsonConvert.SerializeObject(picname));
            connection.Open();
            command.ExecuteNonQuery();

             d = await DBob.Admin.FindAsync(cid);


            ViewBag.ob = d;
            ViewBag.url = picname;
                ViewBag.img = picname;
           

            return View("Owner/Managerprofile");


        }

        //method for manager change password
        public IActionResult Managereditpassword(string cp, string np, string npa)
        {
            int cid = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("Adminsession"));
            command = new SqlCommand("update Admin set Password = @p,Re_Password = @cp where Admin_ID = @id", connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = cid;
            command.Parameters.Add("@p", SqlDbType.NVarChar, 255).Value = np;
            command.Parameters.Add("@cp", SqlDbType.NVarChar, 255).Value = np;
            connection.Open();
            command.ExecuteNonQuery();
            return RedirectToAction("Managerprofile");

        }

        //method  that use to logout
        public IActionResult logout()
        {
            HttpContext.Session.Remove("Adminsession");
            HttpContext.Session.Remove("Adminsession_img");
            return RedirectToAction("Index", "TLC_Admin");
        }


        public async Task<IActionResult> sendEmails([Bind("Imagename", "Email_title", "Email_body")] Email ob)
        {
            int ic = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("ic"));
            int cc = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("cc"));
            int oc = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("oc"));
            int dc = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("dc"));
            int fc = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("fc"));
            int ec = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("ec"));
           
            int sc = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("sc"));
            int moc = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("moc"));
            string Name = null, imgp = null;
            command = new SqlCommand("select Email from customerinfo", connection);
            connection.Open();
            reader = command.ExecuteReader();
            
           
            string rootpath = _hostEnvironment.WebRootPath;
            string picname = Path.GetFileNameWithoutExtension(ob.Imagename.FileName);
            string extension = Path.GetExtension(ob.Imagename.FileName);
            picname = picname + extension;
            string path = Path.Combine(rootpath + "/Images/Manager/" + picname);
            using (var filestream = new FileStream(path, FileMode.Create))
            {

                await ob.Imagename.CopyToAsync(filestream);
            }
            string title = ob.Email_title;
            string body = ob.Email_body;

            string Themessage = @"<html>
                         

                <img src = cid:myImageID style = "" width:600px;height:400px;position:relative;left:180px;margin-left:170px""><br><p>{msg}</p>";







            while (reader.Read())
            {
                string to = reader["Email"].ToString();
                Themessage = Themessage.Replace("{msg}", body);
                // string to = "it20009540@my.sliit.lk"; //To address

                string from = "tlclifepartner2021@gmail.com"; //From address    
                MailMessage message = new MailMessage(from, to);
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(Themessage, null, "text/html");


                LinkedResource theEmailImage = new LinkedResource("wwwroot/Images/Manager/" + picname);
                theEmailImage.ContentId = "myImageID";
                htmlView.LinkedResources.Add(theEmailImage);



                message.AlternateViews.Add(htmlView);

                message.Subject = title;
                //message.Body = mailbody;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
                NetworkCredential MyCredentials = new NetworkCredential("tlclifepartner2021@gmail.com", "Tlc@2021");

                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = MyCredentials;


                client.Send(message);
            }
            reader.Close();
            ViewBag.itemcount = ic;
            ViewBag.cuscount = cc;
            ViewBag.ocount = oc;
            ViewBag.dcount = dc;
            ViewBag.fcount = fc;
            ViewBag.mocount = moc;
            ViewBag.scount = sc;
         
            ViewBag.ecount = ec;
            ViewBag.mpic = imgp;

            int aid = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("Adminsession"));
                command = new SqlCommand("SELECT * FROM Admin where Admin_ID = @id ", connection);
                command.Parameters.Add("@id", SqlDbType.Int).Value = aid;
                reader = command.ExecuteReader();
               
                while (reader.Read())
                {


                   Name = reader["Name"].ToString();
                   imgp = reader["Image"].ToString();
                }
                ViewBag.mpic = imgp;
                DateTime now = DateTime.Now;
                string s = now.DayOfWeek.ToString();
                ViewBag.day = s;
                ViewBag.name = Name;
                ViewBag.TerminoDaAvaliacao = now;

            
            return View("Owner/Owner_dashboard");
        }


        public IActionResult CartPartialView() {

            DateTime now = DateTime.Now;
            
            int cmonth = now.Month;

            command = new SqlCommand("SELECT * FROM orders", connection);
            connection.Open();
            reader = command.ExecuteReader();
            int ocount = reader.Cast<object>().Count();
            reader.Close();

            command = new SqlCommand("SELECT Quntity FROM Orders o, Orderdetails od where o.id = od.OrderId and  datepart(month,OrderDate) = @m ", connection);
            command.Parameters.Add("@m", SqlDbType.Int).Value = cmonth;
            reader = command.ExecuteReader();
            int qtotal = 0;
            while (reader.Read())
            {
                int value = int.Parse(reader["Quntity"].ToString());
                qtotal = qtotal + value;
            }
            reader.Close();

            command = new SqlCommand("SELECT TotalPrice FROM Orders o, Orderdetails od where o.id = od.OrderId and  datepart(month,OrderDate) = @m ", connection);
            command.Parameters.Add("@m", SqlDbType.Int).Value = cmonth;
            reader = command.ExecuteReader();
            int total = 0;
            while (reader.Read())
            {
                int value = int.Parse(reader["TotalPrice"].ToString());
                total = total + value;
            }
            reader.Close();

            ViewBag.cart1 = ocount;
            ViewBag.cart2 = qtotal;
            ViewBag.cart3 = total;

            return PartialView("Owner/CartPartialView");
        }


        public IActionResult managerpartail()
        {
            command = new SqlCommand("select * from admin",connection);
            connection.Open();
            reader = command.ExecuteReader();
            int count = reader.Cast<object>().Count();
            ViewBag.c = count;
            return PartialView("Owner/_manager");
        }

        public IActionResult managerList() {
           List<Admin> ob =  DBob.Admin.ToList();
            return View("Owner/managerList",ob);
        }

        //public JsonResult draw2()
        //{
        //    List<Areagraph> ob = new List<Areagraph>();
        //    Areagraph ob1 = new Areagraph();
        //    command = new SqlCommand("select Cusid from Customerinfo where address like @t", connection);
        //    string place1 = "%Diulapitiya%";
        //    command.Parameters.Add("@id", SqlDbType.NVarChar).Value = place1;
        //    connection.Open();
        //    reader = command.ExecuteReader();
        //    int c1 = reader.Cast<object>().Count();
        //    reader.Close();

        //    ob1.area = "diulapitiya";
        //    ob1.count = c1;
        //    ob.Add(ob1);


        //    command = new SqlCommand("select Cusid from Customerinfo where address like @t", connection);
        //    place1 = "%Diulapitiya%";
        //    command.Parameters.Add("@id", SqlDbType.NVarChar).Value = place1;
        //    reader = command.ExecuteReader();
        //    c1 = reader.Cast<object>().Count();
        //    reader.Close();

        //    ob1.area = "diulapitiya";
        //    ob1.count = c1;
        //    ob.Add(ob1);

        //    command = new SqlCommand("select Cusid from Customerinfo where address like @t", connection);
        //    place1 = "%Diulapitiya%";
        //    command.Parameters.Add("@id", SqlDbType.NVarChar).Value = place1;
        //    reader = command.ExecuteReader();
        //    c1 = reader.Cast<object>().Count();
        //    reader.Close();

        //    ob1.area = "diulapitiya";
        //    ob1.count = c1;
        //    ob.Add(ob1);



        //    command = new SqlCommand("select Cusid from Customerinfo where address like @t", connection);
        //    place1 = "%Diulapitiya%";
        //    command.Parameters.Add("@id", SqlDbType.NVarChar).Value = place1;
        //    reader = command.ExecuteReader();
        //    c1 = reader.Cast<object>().Count();
        //    reader.Close();

        //    ob1.area = "diulapitiya";
        //    ob1.count = c1;
        //    ob.Add(ob1);

        //    return Json(ob);
        //}


        public IActionResult ManagerCreate() {

            return View("Owner/ManagerCreate");
        }

        [HttpPost]
        public async Task<IActionResult> ManagerCreate(string f1, string f2, string f3, string f4, string f5, string f6)
        {
            var pwd = new Password().IncludeLowercase().IncludeUppercase().LengthRequired(5);
            string result = pwd.Next();
            var uname = new Password().IncludeLowercase().LengthRequired(6);
            string runame = uname.Next();
            Admin ob = new Admin();
            ob.Name = f1;
            ob.Email = f2;
            ob.Phone_Number = f3;
            ob.NIC = f4;
            ob.Qualifications = f5;
            ob.Username = runame;
            ob.Password = result;
            ob.Re_Password = result;
            string type = null;
            if (f6.Equals("HR"))
            {
                type = "HR manager";
            }
            else if (f6.Equals("IM"))
            {
                type = "Inventory Manager";
            }
            else if (f6.Equals("SM")) {
                type = "Supplier Manager";
            }
            else
            {
                type = "Delivery Manager";
            }
            ob.Type = type;
            DBob.Add(ob);
            await DBob.SaveChangesAsync();
            string Themessage = @"<html>
                          <body>
                            <div style="" width: 500px; height: 620px; border: 6px solid DodgerBlue;position:relative; "">

                <img src = cid:myImageID style = "" width:150px;height:auto;position:relative;left:180px;margin-left:170px"">
        

                 <h4 style = ""position:relative;left:40px;font-family:Comic Sans MS;margin-left:60px;font-size:20px"" ><font color = ""DodgerBlue""> Hi </font>{name} your are our new {type} </h4>
                  
                         <img src = cid:myImageID2 style = "" width:270px;height:250px;position:absolute;left:300px;border-radius:50%;margin-left:120px"">
                     
                            <h4 style = ""position:relative;left:90px;top:0px;margin-left:170px;font-size:20px""> Username = <font color = ""#9932CC"">{uname}</font> </h4>
                            <h4 style = ""position:relative;left:90px;top:0px;margin-left:170px;font-size:20px""> Password = <font color = ""#9932CC"">{pword}</font> </h4>
                          
                              <p style = ""font-size:20px;color:#FF7F50;position:relative;left:80px;top:0px;letter-spacing:5px;margin-left:70px"" >WELCOME TO TLC FAMILY </p>
                           

                           </div>
                            </body>
                            </html>";








            Themessage = Themessage.Replace("{uname}", runame);
            Themessage = Themessage.Replace("{type}", ob.Type);
            Themessage = Themessage.Replace("{pword}", result);
            Themessage = Themessage.Replace("{name}", ob.Name);
            string to = ob.Email; //To address    
            string from = "tlclifepartner2021@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, to);
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(Themessage, null, "text/html");


            LinkedResource theEmailImage = new LinkedResource("wwwroot/Images/Customer/logo.png");
            theEmailImage.ContentId = "myImageID";
            htmlView.LinkedResources.Add(theEmailImage);

            LinkedResource theEmailImage2 = new LinkedResource("wwwroot/Images/Customer/man.gif");
            theEmailImage2.ContentId = "myImageID2";
            htmlView.LinkedResources.Add(theEmailImage2);

            message.AlternateViews.Add(htmlView);

            message.Subject = "Welcome To TLC Family";
            //message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            NetworkCredential MyCredentials = new NetworkCredential("tlclifepartner2021@gmail.com", "Tlc@2021");

            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = MyCredentials;


            client.Send(message);
            return RedirectToAction("managerList");

        }

        [HttpPost]
        public ActionResult Deleteadmin(int id)
        {
            command = new SqlCommand("delete admin where Admin_ID = @id", connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;
            // command.CommandType = System.Data.CommandType.TableDirect;
            connection.Open();
            command.ExecuteNonQuery();
            return RedirectToAction("managerList");

        }
        public IActionResult errorpage()
        {
            return View("errorpage");
        }

        public IActionResult DriverDash()
        {
            command = new SqlCommand("SELECT * FROM driver", connection);
            connection.Open();
            reader = command.ExecuteReader();
            int dcount = reader.Cast<object>().Count();
            reader.Close();
            ViewBag.Driver = dcount;
            return PartialView("Owner/DriverDash");
        }
    }
}
