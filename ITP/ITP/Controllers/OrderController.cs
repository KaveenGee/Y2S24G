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
        [HttpPost]
        public IActionResult Checkout(List<ViewCartDetailsModel> viewCartDetailsModels)
        {

            //List<OrderDetails> items = HttpContext.Session.Get<List<OrderDetails>>("items");
            List<OrderDetails> orderDetailsList = new List<OrderDetails>();
            foreach (var item in viewCartDetailsModels)
            {
                OrderDetails orderDetails = new OrderDetails();
                orderDetails.ItemId = item.IItemId;
                orderDetails.Quntity = item.Quntity;
                orderDetails.Price = item.IUPrice;
                orderDetails.TotalPrice = item.Quntity * item.IUPrice;
                item.TotalPrice = item.Quntity * item.IUPrice;
                orderDetailsList.Add(orderDetails);
            }
            HttpContext.Session.Set("items", orderDetailsList);

            ViewCartDeliveryDetails viewCartDeliveryDetails = new ViewCartDeliveryDetails();
            viewCartDeliveryDetails.viewCartDetailsModelList = viewCartDetailsModels;
            return View(viewCartDeliveryDetails);
        }

        //Post Checkout action method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessCheckout(ViewCartDeliveryDetails viewCartDeliveryDetails)
        {
            Order order = new Order();
            List<OrderDetails> items = HttpContext.Session.Get<List<OrderDetails>>("items");
            if (items != null)
            {
                //foreach (var item in items)
                //{
                //    OrderDetails orderDetails = new OrderDetails();
                //    orderDetails.ItemId = item.IItemId;
                //    anOrder.OrderDetails.Add(orderDetails);

                //}
                order.OrderDetails = items;
            }

            order.OrderNo = GetOrderNo();
            order.ReceiverName = viewCartDeliveryDetails.ReceiverName;
            order.DeliveryAddress = viewCartDeliveryDetails.DeliveryAddress;
            order.PhoneNo = viewCartDeliveryDetails.PhoneNo;
            order.City = viewCartDeliveryDetails.City;
            order.Email = viewCartDeliveryDetails.Email;

            _db.Orders.Add(order);
            var a = await _db.SaveChangesAsync();
            HttpContext.Session.Set("items", new List<OrderDetails>());
            return RedirectToAction("CheckoutDetails", new { id = order.Id });
            //return View(); 
        }


        //get delivery details and item details
        public IActionResult CheckoutDetails(int? id)
        {
            var order = _db.Orders.FirstOrDefault(c => c.Id == id);


            List<OrderDetails> items = _db.OrderDetails.Where(c => c.OrderId == id).ToList();
            //HttpContext.Session.Get<List<OrderDetails>>("items");
            if (items == null)
            {
                items = new List<OrderDetails>();
            }
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
            ViewCartDeliveryDetails viewCartDeliveryDetails = new ViewCartDeliveryDetails();

            viewCartDeliveryDetails.OrderNo = order.OrderNo;
            viewCartDeliveryDetails.ReceiverName = order.ReceiverName;
            viewCartDeliveryDetails.DeliveryAddress = order.DeliveryAddress;
            viewCartDeliveryDetails.PhoneNo = order.PhoneNo;
            viewCartDeliveryDetails.City = order.City;
            viewCartDeliveryDetails.Email = order.Email;

            viewCartDeliveryDetails.viewCartDetailsModelList = viewCartDetailsModelsList;
            return View(viewCartDeliveryDetails);
        }

        public string GetOrderNo()
        {
            int rowCount = _db.Orders.ToList().Count() + 1;
            return rowCount.ToString("000");
        }
    }
}
