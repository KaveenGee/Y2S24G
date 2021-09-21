using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITP.Models;
using Microsoft.EntityFrameworkCore;
namespace ITP.Controllers
{
    public class ContController : Controller
    {
        private readonly AppDbContext _db;

        public ContController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var displaydata = _db.ContactusTable.ToList();
            return View(displaydata);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(NewCont nec)
        {
            if (ModelState.IsValid)
            {
                _db.Add(nec);
                await _db.SaveChangesAsync();
                return RedirectToAction("Create");
            }
            return View(nec);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var getContdetails = await _db.ContactusTable.FindAsync(id);
            return View(getContdetails);
        }
        [HttpPost]

        public async Task<IActionResult> Edit(NewCont nc)
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
            var getContdetails = await _db.ContactusTable.FindAsync(id);
            return View(getContdetails);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var getContdetails = await _db.ContactusTable.FindAsync(id);
            return View(getContdetails);
        }
        [HttpPost]

        public async Task<IActionResult> Delete(int id)
        {

            var getContdetails = await _db.ContactusTable.FindAsync(id);
            _db.ContactusTable.Remove(getContdetails);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        public IActionResult Ownerdashboard()
        {
            return View();
        }

        public IActionResult MethodC()
        {
            var displaydata = _db.ContactusTable.ToList();
            return PartialView("ContDash", displaydata);
        }

        public IActionResult ContactPartial()
        {

            return PartialView("Owner/ContactDash");
        }
    }


}
