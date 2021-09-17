using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ITP.Models;

namespace ITP.Controllers
{
    public class OrderListController : Controller
    {
        private readonly AppDbContext _db;

        public OrderListController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var displaydata = _db.DeliveryList.ToList();
            return View(displaydata);
        }
        [HttpGet]
        public async Task<IActionResult> Index(string Ordersearch)
        {
            ViewData["Getorderdetails"] = Ordersearch;

            var orderquery = from x in _db.DeliveryList select x;
            if(!String.IsNullOrEmpty(Ordersearch))
            {
                orderquery=orderquery.Where(x=>x.Receivername.Contains(Ordersearch));
            }
            return View(await orderquery.AsNoTracking().ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(OrderListClass nec)
        {
            if(ModelState.IsValid)
            {
                _db.Add(nec);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(nec);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if(id==null)
            {
                return RedirectToAction("Index");
            }
            var getorderdetails = await _db.DeliveryList.FindAsync(id);
            return View(getorderdetails);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(OrderListClass nc)
        {
            if(ModelState.IsValid)
            {
                _db.Update(nc);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(nc);
              
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var getorderdetails = await _db.DeliveryList.FindAsync(id);
            return View(getorderdetails);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var getorderdetails = await _db.DeliveryList.FindAsync(id);
            return View(getorderdetails);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
     
            var getorderdetails = await _db.DeliveryList.FindAsync(id);
            _db.DeliveryList.Remove(getorderdetails);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult DashboardView()
        {
            return View();
        }
        public IActionResult Methodk()
        {
            var displaydata = _db.DeliveryList.ToList();
            return PartialView("OrderDash",displaydata);
        }
    }
}
