using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ITP.Models;

namespace ITP.Controllers
{
    public class excessfaultyController : Controller
    {
        private readonly AppDbContext _db;
        public excessfaultyController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var displaydata = _db.ExcessFaulty.ToList();
            return View(displaydata);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Create(excessfaultyclass nec)
        {
            if (ModelState.IsValid)
            {
                _db.Add(nec);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(nec); 
        }
        public async Task<IActionResult> Edit (int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var getexcessivefaultydetails = await _db.ExcessFaulty.FindAsync(id);
            return View(getexcessivefaultydetails);
        }
        [HttpPost]
        public async Task <IActionResult> Edit(excessfaultyclass nc)
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
            var getexcessivefaultydetails = await _db.ExcessFaulty.FindAsync(id);
            return View(getexcessivefaultydetails);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var getexcessivefaultydetails = await _db.ExcessFaulty.FindAsync(id);
            return View(getexcessivefaultydetails);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var getexcessivefaultydetails = await _db.ExcessFaulty.FindAsync(id);
            _db.ExcessFaulty.Remove(getexcessivefaultydetails);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
