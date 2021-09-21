using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ITP.Models;
using ITP.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ITP.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        private AppDbContext _db;



        public HomeController(AppDbContext db, ILogger<HomeController> logger)
        {
            _db = db;
            _logger = logger;
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
                action4 = "logout";
                
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
            return View(_db.Item.ToList());
        }

        //Get items details action method
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var item = _db.Item.FirstOrDefault(c => c.IItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            int cid = 0;
            string action1, action2, icon, action3, action4 = null, action5, img = null;

            if (HttpContext.Session.GetString("customersession") != null)
            {

                cid = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("customersession"));
                img = JsonConvert.DeserializeObject<string>(HttpContext.Session.GetString("customersession_img"));
                if (String.IsNullOrEmpty(img))
                {
                    action5 = "default.png";
                }
                else
                {
                    action5 = img;
                }
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
                action5 = "plus.png";
            }
            ViewData["action3"] = action3;
            ViewData["action4"] = action4;
            ViewData["icon"] = icon;
            ViewData["action1"] = action1;
            ViewData["action2"] = action2;
            ViewData["cid"] = cid;
            ViewBag.img = action5;

            

            ViewItemDetailsModel viewItemDetailsModel = new ViewItemDetailsModel();
            viewItemDetailsModel.IItemId = item.IItemId;
            viewItemDetailsModel.IBrand = item.IBrand;
            viewItemDetailsModel.IUPrice = item.IUPrice;
            viewItemDetailsModel.IDescription = item.IDescription;
            viewItemDetailsModel.Quntity = 1;
            viewItemDetailsModel.ImageName = item.ImageName;
            return View(viewItemDetailsModel);

        }

        //POST items details action method
        //[HttpPost]
        //[ActionName("Detail")]
        //public ActionResult ItemDetail(int? id)
        //{
        //    List<ItemModel> items = new List<ItemModel>();
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var item = _db.Item.FirstOrDefault(c => c.IItemId == id);
        //    if (item == null)
        //    {
        //        return NotFound();
        //    }
        //    items = HttpContext.Session.Get<List<ItemModel>>("items");
        //    if (items == null)
        //    {
        //        items = new List<ItemModel>();
        //    }
        //    items.Add(item);
        //    HttpContext.Session.Set("items", items);
        //    //return View(item);
        //    return RedirectToAction(nameof(Detail));
        //}





        [HttpPost]
        [ActionName("Detail")]
        public ActionResult ItemDetail(int? id, ViewItemDetailsModel viewItemDetailsModel)
        {

            List<OrderDetails> items = new List<OrderDetails>();
            if (id == null)
            {
                return NotFound();
            }
            //var item = _db.Item.FirstOrDefault(c => c.IItemId == id);
            //if (item == null)
            //{
            //    return NotFound();
            //}
            items = HttpContext.Session.Get<List<OrderDetails>>("items");
            if (items == null)
            {
                items = new List<OrderDetails>();
            }
            OrderDetails od = new OrderDetails();
            od.ItemId = viewItemDetailsModel.IItemId;
            od.Quntity = viewItemDetailsModel.Quntity;
            od.Price = viewItemDetailsModel.IUPrice;
            od.TotalPrice = viewItemDetailsModel.Quntity * viewItemDetailsModel.IUPrice;
            items.Add(od);
            HttpContext.Session.Set("items", items);
            //return View(item);
            return RedirectToAction(nameof(Detail));
        }

        //Get Remove action method
        [ActionName("Remove")]
        public IActionResult RemoveToCart(int? id)
        {
            List<OrderDetails> items = HttpContext.Session.Get<List<OrderDetails>>("items");
            if (items != null)
            {
                var item = items.FirstOrDefault(c => c.ItemId == id);
                if (item != null)
                {
                    items.Remove(item);
                    HttpContext.Session.Set("items", items);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        //remove to cart action 
        [HttpPost]
        public IActionResult Remove(int? id)
        {
            List<OrderDetails> items = HttpContext.Session.Get<List<OrderDetails>>("items");
            if (items != null)
            {
                var item = items.FirstOrDefault(c => c.ItemId == id);
                if (item != null)
                {
                    items.Remove(item);
                    HttpContext.Session.Set("items", items);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        //Get product cart action method

        public IActionResult Cart()
        {
            List<OrderDetails> items = HttpContext.Session.Get<List<OrderDetails>>("items");
            if (items == null)
            {
                items = new List<OrderDetails>();
            }

            int cid = 0;
            string action1, action2, icon, action3, action4 = null, action5, img = null;

            if (HttpContext.Session.GetString("customersession") != null)
            {

                cid = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("customersession"));
                img = JsonConvert.DeserializeObject<string>(HttpContext.Session.GetString("customersession_img"));
                if (String.IsNullOrEmpty(img))
                {
                    action5 = "default.png";
                }
                else
                {
                    action5 = img;
                }
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
                action5 = "plus.png";
            }
            ViewData["action3"] = action3;
            ViewData["action4"] = action4;
            ViewData["icon"] = icon;
            ViewData["action1"] = action1;
            ViewData["action2"] = action2;
            ViewData["cid"] = cid;
            ViewBag.img = action5;

            List<ViewCartDetailsModel> viewCartDetailsModelsList = new List<ViewCartDetailsModel>();
            foreach (var item in items)
            {
                ViewCartDetailsModel viewCartDetailsModel = new ViewCartDetailsModel();

                viewCartDetailsModel.IItemId = item.ItemId;
                viewCartDetailsModel.Quntity = item.Quntity;
                viewCartDetailsModel.IUPrice = item.Price;


                var product = _db.Item.FirstOrDefault(c => c.IItemId == item.ItemId);

                viewCartDetailsModel.IBrand = product.IBrand;
                viewCartDetailsModel.IDescription = product.IDescription;
                viewCartDetailsModel.ImageName = product.ImageName;

                viewCartDetailsModelsList.Add(viewCartDetailsModel);
            }




            return View(viewCartDetailsModelsList);
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

        public IActionResult errorpage() {

            return View();
        }

       
    }
}
