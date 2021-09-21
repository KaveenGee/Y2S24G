using ITP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ITP.Controllers
{
    public class CartController : Controller
    {
        SqlCommand command;
        private static String connectionstring = "workstation id=Project.mssql.somee.com;packet size=4096;user id=donkavi2_SQLLogin_1;pwd=12345678;data source=Project.mssql.somee.com;persist security info=False;initial catalog=Project";
        SqlConnection connection = new SqlConnection(connectionstring);
        SqlDataReader reader;
        
        private readonly AppDbContext _db;

        public CartController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            command = new SqlCommand("select Od.id, Orderid, OrderNo, itemid, IBrand,IModel,Quntity,Price,TotalPrice,PhoneNo,ReceiverName,OrderDate,ImageName from OrderDetails Od, Item I, Orders O where I.IItemId = Od.ItemId and  Od.OrderId = O.Id ", connection);
            connection.Open();
            CartClass test = null;
            List<CartClass> TestList = new List<CartClass>();
            reader = command.ExecuteReader();

            String Image = null;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    test = new CartClass();
                    test.Id = int.Parse(reader["Id"].ToString());
                    test.OrderId = int.Parse(reader["OrderId"].ToString());
                    test.OrderNo = int.Parse(reader["OrderNo"].ToString());
                    test.ItemId = int.Parse(reader["ItemId"].ToString());
                    test.IBrand = reader["IBrand"].ToString();
                    test.IModel = reader["IModel"].ToString();
                    test.Quntity = int.Parse(reader["Quntity"].ToString());
                    test.Price = int.Parse(reader["Price"].ToString());
                    test.TotalPrice = int.Parse(reader["TotalPrice"].ToString());
                    test.PhoneNo = reader["PhoneNo"].ToString();
                    test.ReceiverName = reader["ReceiverName"].ToString();
                    test.OrderDate = DateTime.Parse(reader["OrderDate"].ToString());
                    test.ImageName = reader["ImageName"].ToString();
                    

                    TestList.Add(test);
                }
                ViewBag.ob = TestList;
                
            }
                return View(TestList);
        }

        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(CartClass nec)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _db.Add(nec);
        //        await _db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(nec);

        //}

        public async Task<IActionResult> Edit(int? id)
        {
            command = new SqlCommand(" select Od.id, Orderid, OrderNo, itemid, IBrand,IModel,Quntity,Price,TotalPrice,PhoneNo,ReceiverName,OrderDate,ImageName  from OrderDetails Od, Item I, Orders O where I.IItemId = Od.ItemId and  Od.OrderId = O.Id and Od.Id = @id", connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;
            connection.Open();

            CartClass test = null;
            List<CartClass> TestList = new List<CartClass>();

            String Image = null;

            reader = command.ExecuteReader();
            if (id == null)
           {
               return RedirectToAction("Index");
            }
            while (reader.Read())
            {
                test = new CartClass();
                test.Id = int.Parse(reader["Id"].ToString());
                test.OrderId = int.Parse(reader["OrderId"].ToString());
                test.OrderNo = int.Parse(reader["OrderNo"].ToString());
                test.ItemId = int.Parse(reader["ItemId"].ToString());
                test.IBrand = reader["IBrand"].ToString();
                test.IModel = reader["IModel"].ToString();
                test.Quntity = int.Parse(reader["Quntity"].ToString());
                test.Price = int.Parse(reader["Price"].ToString());
                test.TotalPrice = int.Parse(reader["TotalPrice"].ToString());
                test.PhoneNo = reader["PhoneNo"].ToString();
                test.ReceiverName = reader["ReceiverName"].ToString();
                test.OrderDate = DateTime.Parse(reader["OrderDate"].ToString());
                Image = reader["ImageName"].ToString();

            }

            ViewBag.img = Image;
            return View(test);
       }

        [HttpPost]
       public async Task<IActionResult> Edit(CartClass nc)
       {
            command = new SqlCommand(" update orders set ReceiverName = @rname, PhoneNo = @rphoneno where OrderNo = @no", connection);
            command.Parameters.Add("@no", SqlDbType.Int).Value = nc.OrderNo;
            command.Parameters.Add("@rname", SqlDbType.NVarChar, 150).Value = nc.ReceiverName;
            command.Parameters.Add("@rphoneno", SqlDbType.NVarChar, 20).Value = nc.PhoneNo;
            connection.Open();

            command.ExecuteNonQuery();
       
               return RedirectToAction("Index");
         
       }

        public async Task<IActionResult> Details(int? id)
        {
            command = new SqlCommand(" select Od.id, Orderid, OrderNo, itemid, IBrand,IModel,Quntity,Price,TotalPrice,PhoneNo,ReceiverName,OrderDate,ImageName from OrderDetails Od, Item I, Orders O where I.IItemId = Od.ItemId and  Od.OrderId = O.Id and Od.Id = @id", connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;
            connection.Open();
            CartClass test = null;
            List<CartClass> TestList = new List<CartClass>();
            reader = command.ExecuteReader();

            String Image = null;

            if (id == null)
            {
                return RedirectToAction("Index");
            }
            while (reader.Read())
            {
                test = new CartClass();
                test.Id = int.Parse(reader["Id"].ToString());
                test.OrderId = int.Parse(reader["OrderId"].ToString());
                test.OrderNo = int.Parse(reader["OrderNo"].ToString());
                test.ItemId = int.Parse(reader["ItemId"].ToString());
                test.IBrand = reader["IBrand"].ToString();
                test.IModel = reader["IModel"].ToString();
                test.Quntity = int.Parse(reader["Quntity"].ToString());
                test.Price = int.Parse(reader["Price"].ToString());
                test.TotalPrice = int.Parse(reader["TotalPrice"].ToString());
                test.PhoneNo = reader["PhoneNo"].ToString();
                test.ReceiverName = reader["ReceiverName"].ToString();
                test.OrderDate = DateTime.Parse(reader["OrderDate"].ToString());
                Image = reader["ImageName"].ToString();

            }
            ViewBag.img = Image;
            return View(test);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            command = new SqlCommand(" select Od.id, Orderid, OrderNo, itemid, IBrand,IModel,Quntity,Price,TotalPrice,PhoneNo,ReceiverName,OrderDate,ImageName from OrderDetails Od, Item I, Orders O where I.IItemId = Od.ItemId and  Od.OrderId = O.Id and Od.Id = @id", connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;
            connection.Open();

            CartClass test = null;
            List<CartClass> TestList = new List<CartClass>();
            reader = command.ExecuteReader();

            String Image = null;

            if (id == null)
            {
                return RedirectToAction("Index");
            }
            while (reader.Read())
            {
                test = new CartClass();
                test.Id = int.Parse(reader["Id"].ToString());
                test.OrderId = int.Parse(reader["OrderId"].ToString());
                test.OrderNo = int.Parse(reader["OrderNo"].ToString());
                test.ItemId = int.Parse(reader["ItemId"].ToString());
                test.IBrand = reader["IBrand"].ToString();
                test.IModel = reader["IModel"].ToString();
                test.Quntity = int.Parse(reader["Quntity"].ToString());
                test.Price = int.Parse(reader["Price"].ToString());
                test.TotalPrice = int.Parse(reader["TotalPrice"].ToString());
                test.PhoneNo = reader["PhoneNo"].ToString();
                test.ReceiverName = reader["ReceiverName"].ToString();
                test.OrderDate = DateTime.Parse(reader["OrderDate"].ToString());
                Image = reader["ImageName"].ToString();

            }

            ViewBag.img = Image;
            return View(test);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string OD)
        {
            int id = int.Parse(OD);
            command = new SqlCommand("delete orderdetails where OrderId = @odid", connection);
            command.Parameters.Add("@odid", SqlDbType.Int).Value = id;
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            command = new SqlCommand("delete orders where id = @oid", connection);
            command.Parameters.Add("@oid", SqlDbType.Int).Value = id;
            connection.Open();
            command.ExecuteNonQuery();
           
          
            return RedirectToAction("Index");
        }
        
    }
}

