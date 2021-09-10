using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ITP.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
          
            return PartialView("Owner/_dashboard");
        }

       

        //render the owner dashboard partial view2
        public ActionResult CustomerList()
        {
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
          ViewBag.totalcustomer = TestList;
            return View("Owner/CustomerList");
        }


      

        //method for add new customer by admin modal
        [HttpPost]
        public ActionResult saveCustomer(Customer ob)
        {

            DBob.CustomerInfo.Add(ob);
            DBob.SaveChanges();
            

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
            connection.Close();
            ViewBag.totalcustomer = TestList;
            return View("Owner/CustomerList");
         
           
           

            
        }

        //reder customer partial view
        public IActionResult _customer() {
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
                page = "Owner/Owner_dashboard";
            }
            else if (type1.Equals("IM")) {
                type1 = "Inventory Manager";
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
                string img = JsonConvert.DeserializeObject<string>(HttpContext.Session.GetString("Adminsession_img"));
                if (String.IsNullOrEmpty(img))
                {
                    imgp = "default.png";
                }
                else
                {
                    imgp = test.Image;
                }

                command = new SqlCommand("SELECT * FROM item", connection);
                connection.Open();
                reader = command.ExecuteReader();
                int count = reader.Cast<object>().Count();
                ViewBag.itemcount = count;
                ViewBag.mpic = imgp;
                DateTime now = DateTime.Now;
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

        public async Task<IActionResult> render2()
        {
            int cid = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("Adminsession"));
            var model = await DBob.Admin.FindAsync(cid);
            ViewBag.ob = model;
            return PartialView("Owner/_manageredit");
        }

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

    }
}
