using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITP.Models;
using Microsoft.EntityFrameworkCore;

namespace Handlesuppliers.Controllers
{
    public class handlesupplierController : Controller
    {
        private readonly AppDbContext _db;

        public handlesupplierController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var displaydata = _db.handlesuppliers.ToList();
            return View(displaydata);
        }
        [HttpGet]
        public async Task<IActionResult> Index(string Suppliersearch)
        {
            ViewData["GetSupplierdetails"] = Suppliersearch;

            var orderquery = from x in _db.handlesuppliers select x;
            if (!String.IsNullOrEmpty(Suppliersearch))
            {
                orderquery = orderquery.Where(x => x.NameofSupplier.Contains(Suppliersearch));
            }
            return View(await orderquery.AsNoTracking().ToListAsync());
        }


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(handlesupplierClass nec)
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
            if(id == null)
            {
                return RedirectToAction("Index");
            }
            var gethandlesupplierdetails = await _db.handlesuppliers.FindAsync(id);
            return View(gethandlesupplierdetails);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(handlesupplierClass nc)
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
            var gethandlesupplierdetails = await _db.handlesuppliers.FindAsync(id);
            return View(gethandlesupplierdetails);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var gethandlesupplierdetails = await _db.handlesuppliers.FindAsync(id);
            return View(gethandlesupplierdetails);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
           
            var gethandlesupplierdetails = await _db.handlesuppliers.FindAsync(id);
            _db.handlesuppliers.Remove(gethandlesupplierdetails);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
