using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ITP.Models;

namespace ITP.Controllers
{
    public class supplyController : Controller
    {
        private readonly AppDbContext _db;

        public supplyController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var displaydata = _db.Supplierlog.ToList();
            return View(displaydata);
        }
        [HttpGet]
        public async Task<IActionResult> Index(string SupplierLogsearch)
        {
            ViewData["GetSupplierdetails"] = SupplierLogsearch;

            var orderquery = from x in _db.Supplierlog select x;
            if (!String.IsNullOrEmpty(SupplierLogsearch))
            {
                orderquery = orderquery.Where(x => x.Model.Contains(SupplierLogsearch));
            }
            return View(await orderquery.AsNoTracking().ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(supplierlog nec)
        {
            
            if (ModelState.IsValid)
            {
                _db.Add(nec);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(nec);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var getorderdetails = await _db.Supplierlog.FindAsync(id);
            return View(getorderdetails);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(supplierlog nc)
        {
            if (ModelState.IsValid)
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
            var getorderdetails = await _db.Supplierlog.FindAsync(id);
            return View(getorderdetails);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var getorderdetails = await _db.Supplierlog.FindAsync(id);
            return View(getorderdetails);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            var getorderdetails = await _db.Supplierlog.FindAsync(id);
            _db.Supplierlog.Remove(getorderdetails);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
