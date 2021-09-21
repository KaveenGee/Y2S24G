using ITP.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ITP.Controllers
{
    public class ItemController : Controller
    {
        SqlCommand command;
        private static String connectionstring = "workstation id=Project.mssql.somee.com;packet size=4096;user id=donkavi2_SQLLogin_1;pwd=12345678;data source=Project.mssql.somee.com;persist security info=False;initial catalog=Project";
        SqlConnection connection = new SqlConnection(connectionstring);
        SqlDataReader reader;
        List<Customer> TestList = new List<Customer>();
        Customer test = null;

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ItemController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        //GET: Image
        public async Task<IActionResult> Index()
        {
            return View(await _context.Item.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Index(string Itemsearch)
        {
            ViewData["Getitemdetails"] = Itemsearch;

            var itemquery = from x in _context.Item select x;
            if (!String.IsNullOrEmpty(Itemsearch))
            {
                itemquery = itemquery.Where(x => x.IBrand.Contains(Itemsearch) || x.IModel.Contains(Itemsearch) || x.ICategory.Contains(Itemsearch));
            }
            return View(await itemquery.AsNoTracking().ToListAsync());
        }

        public IActionResult DashSample()
        {
            return View();
        }

        public IActionResult _dashboard()
        {
            command = new SqlCommand("select * from Item", connection);
            connection.Open();
            reader = command.ExecuteReader();
            int count = reader.Cast<object>().Count();

            ViewBag.count = count;
            return PartialView("_dashboard");
        }

        public IActionResult _inventory()
        {
            return PartialView("_inventory");
        }

        //GET: Image/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemModel = await _context.Item
                 .FirstOrDefaultAsync(m => m.IItemId == id);
            if (itemModel == null)
            {
                return NotFound();
            }

            return View(itemModel);
        }

        //GET: Image/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Image/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?Linked=317598.
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create([Bind("IItemId,IDateTime,IBrand,IModel,ICategory,IQPurchase,IQStock,IQSold,IUPrice,ITPrice,IIValue,IDiscount,IDescription,ImageFile")] ItemModel itemModel)
        {
            if (ModelState.IsValid)
            {
                //Save Image to wwwroot/image
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(itemModel.ImageFile.FileName);
                string extension = Path.GetExtension(itemModel.ImageFile.FileName);
                itemModel.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension; 
                string path = Path.Combine(wwwRootPath + "/Images/Items", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await itemModel.ImageFile.CopyToAsync(fileStream);
                }
                //Insert record
                _context.Add(itemModel);
                await _context.SaveChangesAsync();
                TempData["save"] = "This Product has been added";
                return RedirectToAction(nameof(Index));
            }
            return View(itemModel);
        }

        //GET: Image/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var itemModel = await _context.Item.FindAsync(id);
            return View(itemModel);
        }

        //POST: Image/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?Linked=317598.
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IItemId,IDateTime,IBrand,IModel,ICategory,IQPurchase,IQStock,IQSold,IUPrice,ITPrice,IIValue,IDiscount,IDescription,ImageName")] ItemModel itemModel)
        {
            if (id != itemModel.IItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemModel);
                    await _context.SaveChangesAsync();
                    TempData["edit"] = "This Product has been Updated";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageModelExists(itemModel.IItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(itemModel);
        }

        private bool ImageModelExists(int itemId)
        {
            throw new NotImplementedException();
        }



        //GET: Image/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemModel = await _context.Item
                .FirstOrDefaultAsync(m => m.IItemId == id);
            if (itemModel == null)
            {
                return NotFound();
            }

            return View(itemModel);
        }

        //POST: Image?Delete/5
        [HttpPost, ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemModel = await _context.Item.FindAsync(id);

            //delete image from wwwroot/image
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images/items", itemModel.ImageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
            //delete the record
            _context.Item.Remove(itemModel);
            await _context.SaveChangesAsync();
            TempData["delete"] = "This Product has been deleted";
            return RedirectToAction(nameof(Index));
        }
    }
}
