using ITP.Models;
using ITP.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITP.Controllers
{
    public class OrderController : Controller
    {
        private AppDbContext _db;

        public OrderController(AppDbContext db)
        {
            _db = db;
        }

        //Get check out action method
        public IActionResult Checkout()
        {
            return View();
        }

        //Post Checkout action method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order anOrder)
        {
            List<ItemModel> items = HttpContext.Session.Get<List<ItemModel>>("items");
            if (items != null)
            {
                foreach (var item in items)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.ItemId = item.IItemId;
                    anOrder.OrderDetails.Add(orderDetails);

                }

            }
            anOrder.OrderNo = GetOrderNo();
            _db.Orders.Add(anOrder);
            await _db.SaveChangesAsync();
            HttpContext.Session.Set("items", new List<ItemModel>());
            //return RedirectToAction("Index");
            return View(); 
        }

        public string GetOrderNo()
        {
            int rowCount = _db.Orders.ToList().Count()+1;
            return rowCount.ToString("000");
        }
    }
}
