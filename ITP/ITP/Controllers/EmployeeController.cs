using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ITP.Models;

namespace ITP.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly Models.AppDbContext _db;


        public EmployeeController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            //List<NewEmployeeClass> displaydata = _db.EmployeeTable.ToList();
            var ob = _db.Employee.ToList();
            return View(ob);
        }

        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeClass nec)
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
            var getempdetails = await _db.Employee.FindAsync(id);
            return View(getempdetails);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeClass nc)
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
            var getempdetails = await _db.Employee.FindAsync(id);
            return View(getempdetails);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var getempdetails = await _db.Employee.FindAsync(id);
            return View(getempdetails);
        }
        
        public async Task<IActionResult>Delete(int id)
        {
            var getempdetails = await _db.Employee.FindAsync(id);
            _db.Employee.Remove(getempdetails);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
